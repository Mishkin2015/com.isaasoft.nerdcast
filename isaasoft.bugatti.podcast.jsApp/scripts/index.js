/// <reference path="lib/ionic/js/angular/angular-resource.js" />

// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.
( function () {
    "use strict";

    /* ----------- cordova ------------*/

    document.addEventListener( 'deviceready', onDeviceReady.bind( this ), false );

    function onDeviceReady() {
        // Handle the Cordova pause and resume events
        document.addEventListener( 'pause', onPause.bind( this ), false );
        document.addEventListener( 'resume', onResume.bind( this ), false );

        // TODO: Cordova has been loaded. Perform any initialization that requires Cordova here. 
    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
    };

    function onResume() {
        // TODO: This application has been reactivated. Restore application state here.
    };

    /* ------------ ionic --------------- */

    var app = angular.module( 'root', ['ionic'] );

    app.run( function ( $ionicPlatform ) {
        $ionicPlatform.ready( function () {
            // for ios7 style header bars
            if ( window.StatusBar ) {
                // org.apache.cordova.statusbar required
                StatusBar.styleLightContent();
            }
            // hide the prev/next buttons on the keyboard input
            if ( window.cordova && window.cordova.plugins && window.cordova.plugins.Keyboard ) {
                cordova.plugins.Keyboard.hideKeyboardAccessoryBar( true );
            }
            // hide the splash screen only after everything's ready (avoid flicker)
            // requires keyboard plugin and confix.xml entry telling the splash screen to stay until explicitly told
            if ( navigator.splashscreen ) {
                navigator.splashscreen.hide();
            }
        } );
    } )

    .directive( 'autofocus', ['$timeout', function ( $timeout ) {
        return {
            restrict: 'A',
            link: function ( $scope, $element ) {
                $timeout( function () {
                    $element[0].focus();
                } );
            }
        }
    }] )

    .config( function ( $compileProvider ) {
        // Set the whitelist for certain URLs just to be safe
        $compileProvider.aHrefSanitizationWhitelist( /^\s*(https?|ftp|mailto|file|tel):/ );
    } )

    .config( function ( $stateProvider, $urlRouterProvider ) {
        $stateProvider.state( 'app', {
            abstract: true,
            views: {
                rootContent: {
                    templateUrl: 'templates/podcast.html',
                    controller: 'AppController'
                }
            }
        } )

        .state( 'app.podcast', {
            abstract: true,
            views: {
                content: {
                    template: '<ion-nav-view></ion-nav-view>'
                }
            }
        } )

        .state( 'app.podcast.list', {
            url: '/podcast',
            views: {
                '': {
                    templateUrl: 'templates/podcast.list.html',
                    controller: 'ListController'
                }
            }
        } )

        .state( 'app.podcast.detail', {
            url: '/podcast/:hashKey',
            views: {
                '': {
                    templateUrl: 'templates/podcast.detail.html',
                    controller: 'DetailController'
                }
            }
        } )

        .state( 'app.podcast.about', {
            url: '/about',
            views: {
                '': {
                    templateUrl: 'templates/podcast.about.html',
                    controller: 'AboutController'
                }
            }
        } );

        $urlRouterProvider.otherwise( "/podcast" );
    } )

    .controller( 'AppController', function ( $scope, $state, $ionicPopover, $ionicPopup, $timeout, AppHelper, JovemNerdPodcast ) {
        $ionicPopover.fromTemplateUrl( 'templates/podcast.menu-right-popover.html', {
            scope: $scope,
        } ).then( function ( popover ) {
            $scope.menuRightPopover = popover;
        } );

        $ionicPopover.fromTemplateUrl( 'templates/podcast.menu-player-popover.html', {
            scope: $scope,
        } ).then( function ( popover ) {
            $scope.menuPlayerPopover = popover;
        } );

        $scope.player = {
            media: {
                MEDIA_NONE: 0,
                MEDIA_STARTING: 1,
                MEDIA_RUNNING: 2,
                MEDIA_PAUSED: 3,
                MEDIA_STOPPED: 4,
            },
            audioPlayer: null,
            audioPlayerStatus: 0,
            podcast: null,
            seekingRange: document.getElementById( 'seekingRange' ),
            totalTime: 0,
            currentTime: 0,
            seekingValue: null,
            seekingValueChangeDate: null,
            totalTimeTimeout: null,
            currentTimeTimeout: null,
            seekingTimeout: null,
            formatTimeValue: function ( value, isHour ) {
                var returnValue = value + '';

                if ( !isHour ) {
                    while ( returnValue.length < 2 ) returnValue = '0' + returnValue;
                }
                else if ( !value ) value = '0';

                return returnValue;
            },
            totalTimeString: function () {
                var time = new TimeSpan( $scope.player.totalTime || 0 );
                var value = '';

                if ( time.hours() )
                    value = $scope.player.formatTimeValue( time.hours(), true ) + ':';

                return value + $scope.player.formatTimeValue( time.minutes() ) + ':' + $scope.player.formatTimeValue( time.seconds() );
            },
            currentTimeString: function () {
                var time = new TimeSpan( $scope.player.currentTime || 0 );
                var value = '';

                if ( time.hours() || $scope.player.totalTimeString().length >= 7 )
                    value = $scope.player.formatTimeValue( time.hours(), true ) + ':';

                return value + $scope.player.formatTimeValue( time.minutes() ) + ':' + $scope.player.formatTimeValue( time.seconds() );
            },
            selectedItem: function () {
                return JovemNerdPodcast.selectedItem();
            },
            startPlayer: function ( podcastItem ) {
                return AppHelper.defer( function ( resolve, reject ) {
                    $scope.player.stop().then( function ( suc ) {
                        $scope.player.podcast = JovemNerdPodcast.selectedItem( podcastItem );

                        $scope.player.audioPlayer = new Media( $scope.player.podcast.Enclosure.Url,
                            function ( x, y, z ) {
                                console.log( 'suc', { x: x, y: y, z: z } );
                            }, function ( x, y, z ) {
                                console.log( 'err', { x: x, y: y, z: z } );
                            }, function ( status ) {
                                $scope.player.audioPlayerStatus = status;
                            } );

                        $scope.player.audioPlayer.play();

                        $scope.player.totalTimeHandler();
                        $scope.player.currentTimeHandler();

                        JovemNerdPodcast.viewPlayer( true );
                        $scope.$broadcast( 'scroll.resize' );

                        resolve( suc );
                    }, function ( err ) {
                        reject( err );
                    } );
                } );
            },
            togglePlay: function () {
                return AppHelper.defer( function ( resolve, reject ) {
                    var podcast = JovemNerdPodcast.selectedItem();

                    if ( $scope.player.canPause() || $scope.player.canPlay() ) {
                        if ( $scope.player.canPause() ) $scope.player.audioPlayer.pause();
                        else if ( $scope.player.canPlay() ) $scope.player.audioPlayer.play();

                        JovemNerdPodcast.viewPlayer( true );
                        $scope.$broadcast( 'scroll.resize' );
                    }

                    resolve();
                } );
            },
            stop: function () {
                return AppHelper.defer( function ( resolve, reject ) {
                    if ( $scope.player.audioPlayer ) {
                        $scope.player.audioPlayer.stop();
                        $scope.player.audioPlayer.release();

                        $scope.player.audioPlayer = null;

                        JovemNerdPodcast.viewPlayer( false );

                        $scope.$broadcast( 'scroll.resize' );
                    }

                    $scope.player.podcast = null;
                    $scope.player.totalTime = 0;
                    $scope.player.currentTime = 0;

                    resolve();
                } );
            },
            seeking: function () {
                if ( $scope.player.seekingTimeout ) {
                    $timeout.cancel( $scope.player.seekingTimeout );
                }

                if ( $scope.player.currentTimeTimeout ) {
                    $timeout.cancel( $scope.player.currentTimeTimeout );
                }

                $scope.player.seekingValue = seekingRange.value;
                $scope.player.seekingValueChangeDate = DateTime.utcNow();

                return $scope.player.seekingTimeout = $timeout( function () {
                    var longAgo = null;

                    if ( $scope.player.seekingValueChangeDate ) {
                        longAgo = DateTime.utcNow().subtractDate( $scope.player.seekingValueChangeDate );
                    }

                    // seek
                    if ( $scope.player.seekingValue && $scope.player.seekingValue >= 0 && longAgo ) {
                        $scope.player.currentTime = $scope.player.seekingValue; // change to preview

                        if ( longAgo.totalMilliseconds() > 2000 ) {
                            $scope.player.audioPlayer.seekTo( $scope.player.seekingValue );
                            $scope.player.seekingValue = null;
                            $scope.player.seekingValueChangeDate = null;
                        }
                    }

                    $scope.player.currentTimeHandler();
                }, 2000 );
            },
            canPlay: function () {
                return $scope.player.audioPlayerStatus == $scope.player.media.MEDIA_PAUSED ||
                       $scope.player.audioPlayerStatus == $scope.player.media.MEDIA_STOPPED;
            },
            canPause: function () {
                return $scope.player.audioPlayerStatus == $scope.player.media.MEDIA_STARTING ||
                       $scope.player.audioPlayerStatus == $scope.player.media.MEDIA_RUNNING;
            },
            canStop: function () {
                return $scope.player.audioPlayerStatus != $scope.player.media.MEDIA_NONE &&
                       $scope.player.audioPlayerStatus != $scope.player.media.MEDIA_STOPPED;
            },
            viewPlayer: function () {
                return JovemNerdPodcast.viewPlayer();
            },
            totalTimeHandler: function () {
                return AppHelper.defer( function ( resolve, reject ) {
                    if ( $scope.player.audioPlayer ) {
                        var duration = $scope.player.audioPlayer.getDuration();

                        if ( duration > 0 ) {
                            var milliseconds = duration * 1000.0; // duration in seconds

                            $scope.player.totalTime = milliseconds;
                            resolve( milliseconds );
                        }
                        else {
                            $scope.player.totalTimeTimeout = $timeout( $scope.player.totalTimeHandler, 100 );
                        }
                    }
                } );
            },
            currentTimeHandler: function () {
                return AppHelper.defer( function ( resolve, reject ) {
                    if ( $scope.player.audioPlayer ) {
                        // get position
                        var tryAgain = function () {
                            $scope.player.currentTimeTimeout = $timeout( $scope.player.currentTimeHandler, 250 );
                        };

                        $scope.player.audioPlayer.getCurrentPosition(
                            function ( position ) {
                                if ( position > -1 ) {
                                    var milliseconds = position * 1000.0; // position in seconds

                                    $scope.player.currentTime = milliseconds;
                                    resolve( milliseconds );
                                }

                                tryAgain();
                            }, function ( err ) {
                                reject( err );

                                tryAgain();
                            } );
                    }
                } );
            }
        };

        $scope.search = {
            searchInput: function () {
                return document.getElementById( 'searchInput' );
            },
            showSubHeader: false,
            filter: JovemNerdPodcast.filter(),
            goFilterCallback: [],
            toggleShowSubHeader: function () {
                $scope.search.showSubHeader = !$scope.search.showSubHeader;
                if ( $scope.search.showSubHeader ) $scope.search.filter = JovemNerdPodcast.filter();
            },
            hideSubHeader: function () {
                if ( $scope.search.showSubHeader ) $scope.search.toggleShowSubHeader();
            },
            filterChange: function ( event ) {
                if ( event.keyCode == 13 ) $scope.search.goFilter();
            },
            clearButtonClick: function () {
                if ( $scope.search.filter ) {
                    $scope.search.filter = '';

                    $timeout( function () {
                        $scope.search.searchInput().focus();
                    }, 0 );
                }
                else $scope.search.hideSubHeader();
            },
            goFilter: function ( filter ) {
                if ( angular.isDefined( filter ) ) $scope.search.filter = filter;

                JovemNerdPodcast.filter( $scope.search.filter );

                for ( var i in $scope.search.goFilterCallback ) {
                    $scope.search.goFilterCallback[i]();
                }

                $scope.menuRightPopover.hide();
                $scope.search.hideSubHeader();
            }
        };

        /* menus */

        $scope.openMenuRightPopover = function ( event ) {
            $scope.menuRightPopover.show( event );
        };

        $scope.openMenuPlayerPopover = function ( event ) {
            $scope.menuPlayerPopover.show( event );
        };

        $scope.goAbout = function () {
            $scope.menuRightPopover.hide();
            return $state.go( 'app.podcast.about' );
        };

        $scope.goDetail = function () {
            $scope.menuPlayerPopover.hide();
            return $state.go( 'app.podcast.detail', { hashKey: JovemNerdPodcast.selectedItem().HashKey } );
        };

        $scope.$on( '$destroy', function () {
            $scope.menuRightPopover.remove();
            $scope.menuPlayerPopover.remove();
        } );
    } )

    .controller( 'ListController', function ( $scope, $state, $ionicPopup, $ionicScrollDelegate, AppHelper, JovemNerdPodcast ) {
        $scope.podcast = JovemNerdPodcast.allDataSync();
        $scope.doLoadError = false;
        $scope.doLoadReason = null;
        $scope.search.goFilterCallback.push( function () {
            JovemNerdPodcast.init();
            $scope.podcast = JovemNerdPodcast.allDataSync();

            $ionicScrollDelegate.resize();
            $ionicScrollDelegate.scrollTop( 0 );
        } );

        $scope.goDetail = function ( index ) {
            return $state.go( 'app.podcast.detail', { hashKey: JovemNerdPodcast.getItem( index ).HashKey } );
        };

        $scope.pages = {
            hasPages: function () {
                return JovemNerdPodcast.hasPages();
            },
            hasNextUrl: function () {
                return JovemNerdPodcast.hasNextUrl();
            },
            canGetAsync: function () {
                return JovemNerdPodcast.canGetAsync();
            },
            getting: function () {
                return JovemNerdPodcast.getting();
            }
        };

        $scope.doRefresh = function () {
            JovemNerdPodcast.init();

            $scope.doLoadError = false;
            $scope.doLoadReason = null;

            var promise = $scope.doLoad();

            if ( promise ) {
                return promise.finally( function () {
                    $scope.$broadcast( 'scroll.refreshComplete' );
                    $scope.$broadcast( 'scroll.resize' );
                } );
            }

            $scope.$broadcast( 'scroll.refreshComplete' );
            $scope.$broadcast( 'scroll.resize' );

            return AppHelper.deferResolve();
        };

        $scope.canDoLoad = function () {
            return !$scope.doLoadError && ( !JovemNerdPodcast.hasPages() || JovemNerdPodcast.hasNextUrl() );
        };

        $scope.doLoad = function () {
            console.log( 'hasPages', JovemNerdPodcast.hasPages() );
            console.log( 'hasNextUrl', JovemNerdPodcast.hasNextUrl() );
            console.log( 'canGetAsync', JovemNerdPodcast.canGetAsync() );
            console.log( 'getting', JovemNerdPodcast.getting() );

            if ( JovemNerdPodcast.canGetAsync() ) {
                var promise = JovemNerdPodcast.getAsync();

                if ( !promise ) return;

                return promise
                    .then( function ( greeting ) {
                        $scope.podcast = JovemNerdPodcast.allDataSync();
                    }, function ( reason ) {
                        $scope.doLoadError = true;
                        $scope.doLoadReason = reason;
                    } )
                    .finally( function () {
                        $scope.$broadcast( 'scroll.refreshComplete' );
                        $scope.$broadcast( 'scroll.infiniteScrollComplete' );
                        $scope.$broadcast( 'scroll.resize' );
                    } );
            }

            return AppHelper.deferResolve();
        };
    } )

    .controller( 'DetailController', function ( $scope, $stateParams, $timeout, $sce, AppHelper, JovemNerdPodcast ) {
        $scope.$parent.search.hideSubHeader();

        $scope.podcastItem = JovemNerdPodcast.getItemByHashKey( $stateParams.hashKey );
        $scope.gettingDetail = false;
        $scope.detailError = false;
        $scope.detailErrorReason = false;

        if ( !$scope.podcastItem && JovemNerdPodcast.selectedItem().HashKey == $stateParams.hashKey )
            $scope.podcastItem = JovemNerdPodcast.selectedItem();

        $scope.isThisInPlayer = function () {
            var value = $scope.$parent.player.podcast;

            if ( $scope.$parent.player.podcast )
                return $scope.$parent.player.podcast.HashKey == $scope.podcastItem.HashKey;

            return false;
        };

        $scope.canPause = function () {
            return $scope.isThisInPlayer() && $scope.$parent.player.canPause();
        };

        $scope.play = function () {
            if ( $scope.isThisInPlayer() ) {
                $scope.$parent.player.togglePlay();
            }
            else {
                $scope.$parent.player.startPlayer( $scope.podcastItem );
            }
        };

        $scope.podcastDetail = function () {
            var detail = JovemNerdPodcast.detail( $scope.podcastItem.HashKey );
            return detail ? detail.item : null;
        };

        $scope.showDetail = function () {
            $scope.gettingDetail = true;
            $scope.detailError = false;

            return JovemNerdPodcast.getDetailAsync( $scope.podcastItem.HashKey )
                .then( null,
                function ( reason ) {
                    $scope.detailError = true;
                    $scope.detailErrorReason = reason;
                } )
                .finally( function () {
                    $scope.gettingDetail = false;
                } );
        };

        $scope.contentDetail = function () {
            if ( !$scope.podcastDetail() ) return;

            var element = angular.element( '<div>' + $scope.podcastDetail().ContentEncoded + '</div>' );

            element.find( 'img' ).remove();
            var links = element.find( 'a' );

            for ( var i = 0; i < links.length; i++ ) {
                angular.element( links[i] ).attr( 'href', 'javascript:void( 0 );' );
            }

            return element.html();
        };
    } )

    .controller( 'AboutController', function ( $scope, AppHelper, JovemNerdPodcast ) {
        $scope.$parent.search.hideSubHeader();
    } )

    .factory( 'AppHelper', function ( $q ) {
        return {
            defer: function ( action ) {
                var deferred = $q.defer();
                action( deferred.resolve, deferred.reject, deferred.notify );
                return deferred.promise;
            },
            deferResolve: function () {
                var deferred = $q.defer();
                deferred.resolve();
                return deferred.promise;
            },
            deferReject: function ( value ) {
                var deferred = $q.defer();
                deferred.reject( value );
                return deferred.promise;
            }
        };
    } )

    .factory( 'JovemNerdPodcast', function ( $http, $timeout, AppHelper ) {
        var factory = function () {
            var self = this,
                _pages = [],
                _viewPlayer = false,
                _selectedItem = null,
                _getting = false,
                _gettingPromise = null,
                _filter = '',
                _detail = [];

            this.filter = function ( value ) {
                if ( angular.isDefined( value ) ) _filter = value;
                return _filter;
            };

            this.listUrl = function () {
                return 'http://vanquish-podcast.azurewebsites.net/rss/1/item?start=0&limit=10&filter=' + self.filter();
            };

            this.detailUrl = function ( hashKey ) {
                return 'http://vanquish-podcast.azurewebsites.net/rss/1/detail?hashKey=' + hashKey;
            };

            this.viewPlayer = function ( value ) {
                if ( angular.isDefined( value ) ) _viewPlayer = value;
                return _viewPlayer;
            };

            this.getting = function () {
                return _getting;
            };

            this.gettingPromise = function () {
                return _gettingPromise;
            };

            this.hasPages = function () {
                return _pages.length > 0;
            };

            this.hasItems = function () {
                return self.allDataSync().length > 0;
            };

            this.init = function () {
                _pages = [];
                _detail = _detail.slice( _detail.length > 30 ? _detail.length - 30 : 0, 30 );
            };

            this.page = function ( index ) {
                return _pages[index];
            };

            this.selectedItem = function ( value ) {
                if ( angular.isDefined( value ) ) {
                    if ( typeof value == 'number' )
                        _selectedItem = self.getItem( value );
                    else if ( typeof value == 'object' )
                        _selectedItem = value;
                    else throw 'Value is not number or object';
                }
                return _selectedItem;
            };

            this.getItem = function ( index ) {
                if ( !angular.isDefined( index ) || index < 0 ) throw 'not index';
                return self.allDataSync()[index];
            };

            this.getItemByHashKey = function ( hashKey ) {
                if ( !hashKey ) throw 'not hashKey';
                var data = self.allDataSync();
                for ( var i in data ) {
                    if ( data[i].HashKey == hashKey )
                        return data[i];
                }
                return null;
            };

            this.lastPageIndex = function () {
                if ( !self.hasPages() ) throw 'not hasPages';
                return _pages.length - 1;
            };

            this.pageUrlIndex = function ( url ) {
                if ( !url ) throw 'not url';

                for ( var i in _pages ) {
                    if ( _pages[i].Url == url ) return i;
                }

                return;
            };

            this.firstPage = function () {
                if ( !self.hasPages() ) throw 'not hasPages';
                return self.page( 0 );
            };

            this.lastPage = function () {
                if ( !self.hasPages() ) throw 'not hasPages';
                return self.page( self.lastPageIndex() );
            };

            this.hasNextUrl = function () {
                if ( self.hasPages() && self.lastPage().NextUrl )
                    return true;

                return false;
            };

            this.nextUrl = function () {
                if ( self.hasNextUrl() ) return self.lastPage().NextUrl;
                throw 'not hasNextUrl';
            };

            this.canGetAsync = function () {
                return !self.hasPages() || self.hasNextUrl();
            };

            this.allDataSync = function () {
                var items = [];
                for ( var i in _pages ) {
                    var page = _pages[i];
                    for ( var d in page.Data ) {
                        var data = page.Data[d];
                        if ( data ) items.push( data );
                    }
                }
                return items;
            };

            this.getAsync = function () {
                if ( self.getting() ) return;

                _getting = true;

                if ( !self.canGetAsync() ) return AppHelper.deferReject( { status: 'not canGetAsync' } );

                _gettingPromise = AppHelper.defer( function ( resolve, reject, notify ) {
                    var url = self.hasPages() ? self.nextUrl() : self.listUrl();

                    $http.get( url )
                        .success( function ( resp ) {
                            _pages.push( resp );
                            _getting = false;
                            resolve( resp );
                        } )
                        .error( function ( data, status, headers, config ) {
                            _getting = false;
                            reject( {
                                data: data,
                                status: status,
                                headers: headers,
                                config: config
                            } );
                        } );
                } );


                return _gettingPromise;
            };

            this.detail = function ( hashKey ) {
                for ( var i in _detail ) {
                    if ( _detail[i].hashKey == hashKey )
                        return _detail[i];
                }

                return null;
            };

            this.getDetailAsync = function ( hashKey ) {
                if ( !self.detail( hashKey ) ) {
                    var detail = {
                        hashKey: hashKey
                    };

                    var index = _detail.push( detail ) - 1;

                    detail.promise = AppHelper.defer( function ( resolve, reject, notify ) {
                        var url = self.detailUrl( hashKey );

                        $http.get( url )
                            .success( function ( resp ) {
                                detail.item = resp;
                                resolve( resp );
                            } )
                            .error( function ( data, status, headers, config ) {
                                delete _detail[index];
                                reject( {
                                    data: data,
                                    status: status,
                                    headers: headers,
                                    config: config
                                } );
                            } );
                    } );
                }

                return self.detail( hashKey ).promise;
            };
        };

        return new factory();
    } );

} )();