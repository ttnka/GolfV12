// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using GolfV12.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using GolfV12.Client.Servicios.IFaceServ;
using Microsoft.AspNetCore.Components;
using GolfV12.Shared;

namespace GolfV12.Server.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        
/*  yo lo agregre
        [Inject]
        public IG120PlayerServ iPlayerServ { get; set; } 
        public G120Player elPlayer { get; set; } = new G120Player();
hasta aqui
 */ 
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
 /* yo lo agregue
            elPlayer = new G120Player
            {
                UserId = userId,
                Nombre = "Nuevo", Paterno = "Apellido", Materno = " ",Apodo = " ",
                Bday = DateTime.Now, Nivel = (Niveles)3, OrganizacionId = 2,
                Estado = 2, Status = true, Temporal = true
            };
            G120Player elPlayer2 = await iPlayerServ.AddPlayer(elPlayer);
            //elPlayer = await iPlayerServ.AddPlayer(elPlayer);
 hasta aqui
  */ 
            return Page();
        }
    }
}
