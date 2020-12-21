using System;

namespace SmartSchool.API.Helpers
{
  public static class DateTimeExtensions
  {
    public static int GetCurrentAge(this DateTime dateTime)
    {
      if (dateTime == new DateTime())
        return 0;

      int age = DateTime.UtcNow.Year - dateTime.Year;

      if (dateTime.AddYears(age) > DateTime.UtcNow)
        age--;

      return age;
    }
  }
}