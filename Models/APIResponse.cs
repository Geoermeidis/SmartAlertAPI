using System.Net;

namespace SmartAlertAPI.Models
{
    public class APIResponse
    {
        public Object? Result { get; set; }
        public List<string> ErrorMessages { get; set; } = [];
    }
}
