using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsterWebApp.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            if(long.TryParse(this.OrgId, out long _resultschoolId))
            {
                this.OrganizationId = _resultschoolId;
            }
            
        }
        [StringLength(500)]
        public string FirstName { get; set; }
        [StringLength(500)]
        public string LastName { get; set; }
        [StringLength(500)]
        public string Locale { get; set; } = "";

        [StringLength(500)]
        public string OrgId { get; set; }

        private long _schoolId = 0;

        [NotMapped]
        public long OrganizationId { get; set; }
        //{
        //get { return _schoolId; }
        //set
        //{
        //    long.TryParse(this.OrgId, out _schoolId);
        //}
        //}
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
        //public decimal LoyaltyPoint { get; set; } = 0;
        //public decimal LoyaltyPointAmount { get; set; } = 0;
        public int UserTypeId { get; set; } = 1;

        [StringLength(450)]
        public string? EntityId { get; set; }
        public int? CountryId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }

        [StringLength(500)]
        public string? OrgId { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
