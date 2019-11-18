using System.ComponentModel.DataAnnotations;

namespace StudyBuddyBackend.Helpers
{
    public class JwtToken
    {
        [Key]
        public string Token { get; set; }

        public JwtToken()
        {
            
        }
        
        internal JwtToken(string token)
        {
            Token = token;
        }
    }
}
