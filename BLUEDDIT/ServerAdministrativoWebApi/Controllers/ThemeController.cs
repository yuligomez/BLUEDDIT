using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAdministrativoWebApi.Models;

namespace ServerAdministrativoWebApi.Controllers
{
    [Route("v1/api/themes")]
    [ApiController]
    public class ThemeController : ControllerBase
    {
        private readonly ServerAdministrativoManagement management = new ServerAdministrativoManagement();
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ThemeCreationModel model)
        {
            var response = await management.CreateThemeAsync(model.ToEntity(), model.Username);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ThemeUpdateModel model)
        {
            var response = await management.UpdateThemeAsync(model.OldName, model.ToEntity(), model.Username);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] ThemeDeletionModel model)
        {
            var response = await management.DeleteThemeAsync(model.ThemeName, model.Username);
            return Ok(response);
        }
    }
}
