using System.Net;

namespace SmartAlertAPI.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        public Object Result { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
