using System.Text;

namespace SmartSchool.API.Helpers
{
    public static class SharedProps
    {
        public static byte[] TokenKey()
        {
            return Encoding.ASCII.GetBytes("63B847ABD6FDA41D2EB779DB7C35B");
        }
    }
}