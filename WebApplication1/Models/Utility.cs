using System.Text.Json.Serialization;
using System.Text.Json;

namespace WebApplication1.Models
{
    public static class Utility
    {
        public static string ToBase64Image(byte[]? data)
        {
            string imageStr = string.Empty;
            if (data != null)
            {
                imageStr = "data:image/png;base64," + Convert.ToBase64String(data);
			}
            return imageStr;
        }        
    }
}
