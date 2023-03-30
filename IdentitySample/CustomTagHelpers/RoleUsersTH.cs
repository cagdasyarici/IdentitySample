using IdentitySample.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentitySample.CustomTagHelpers
{
    [HtmlTargetElement("td",Attributes = "i-role")]
    public class RoleUsersTH : TagHelper
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public RoleUsersTH(UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            this.userManager = _userManager;
            this.roleManager = _roleManager;
        }

        [HtmlAttributeName("i-role")]
        public string Role { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            IdentityRole role = await roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in userManager.Users)
                {
                    if (user != null && await userManager.IsInRoleAsync(user,role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(
                names.Count == 0 ? "Not Users" :
                string.Join(", ", names));
        }

    }
}
