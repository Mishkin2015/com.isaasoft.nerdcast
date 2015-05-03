using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isaasoft.Core.Helpers
{
    public static class Birth
    {
        public static int Amount(DateTime birth)
        {
            var now = DateTime.Now;
            return Amount(now.Year, now.Month, now.Day, birth.Year, birth.Month, birth.Day);
        }

        public static int Amount(DateTime reference, DateTime birth)
        {
            return Amount(reference.Year, reference.Month, reference.Day, birth.Year, birth.Month, birth.Day);
        }

        public static int Amount(int birthYear, int birthMonth, int birthDay)
        {
            var now = DateTime.Now;
            return Amount(now.Year, now.Month, now.Day, birthYear, birthMonth, birthDay);
        }

        public static int Amount(int referenceYear, int referenceMonth, int referenceDay, int birthYear, int birthMonth, int birthDay)
        {
            var reference = new DateTime(referenceYear, referenceMonth, referenceDay);
            var birthDate = new DateTime(birthYear, birthMonth, birthDay);

            if (reference < birthDate)
            {
                throw new ArgumentException("Reference Date must be greater or equal to the date of birth.");
            }

            var referenceBirthday = default(DateTime);

            if (birthMonth == 2 && birthDay > 28 && !DateTime.IsLeapYear(reference.Year))
            {
                birthDay = 1;
                birthMonth++;
            }

            referenceBirthday = new DateTime(reference.Year, birthMonth, birthDay);

            var amountOfBirthdays = reference.Year - birthDate.Year;

            if (reference < referenceBirthday)
            {
                amountOfBirthdays--;
            }

            return amountOfBirthdays;
        }
    }
}
