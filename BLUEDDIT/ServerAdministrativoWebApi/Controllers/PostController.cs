using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAdministrativoWebApi.Models;

namespace ServerAdministrativoWebApi.Controllers
{
    [Route("v1/api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ServerAdministrativoManagement management = new ServerAdministrativoManagement();
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostCreationModel model)
        {
            var response = await management.CreatePostAsync(model.ToEntity(), model.ThemeName, model.Username);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PostUpdateModel model)
        {
            var response = await management.ModifyPostAsync(model.ToEntity(), model.OldName, model.Username);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] PostDeletionModel model)
        {
            var response = await management.DeletePostAsync(model.PostName, model.Username);
            return Ok(response);
        }

        [Route("associations")]
        [HttpPut]
        public async Task<IActionResult> PutAssociatePostToTheme([FromBody] AssociationModel model)
        {
            var response = await management.AssociatePostToThemeAsync(model.PostName, model.ThemeName, model.Username);
            return Ok(response);
        }

        [Route("dissasociations")]
        [HttpPut]
        public async Task<IActionResult> PutDissasociatePostToTheme([FromBody] DissasociationModel model)
        {
            var response = await management.DissasociatePostToThemeAsync(model.PostName, model.ThemeName, model.Username);
            return Ok(response);
        }
    }
}
