﻿using COMP1640.Services;
using COMP1640.ViewModels.Category.Requests;
using COMP1640.ViewModels.HRM.Requests;
using COMP1640.ViewModels.HRM.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COMP1640.Controllers
{
    [Route("hrm")]
    [Authorize]
    public class HRMController : Controller
    {
        private readonly HRMService _hRMService;
        private readonly CategoryService _categoryService;
        public HRMController(HRMService hRMService, CategoryService categoryService)
        {
            _hRMService = hRMService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetListUserRequest request)
        {
            var vm = await _hRMService.GetListUserAsync(request);
            return View(vm);
        }

        [HttpGet("user/{id:int}")]
        public async Task<IActionResult> GetUserInfo([FromRoute] int id)
        {
            var vm = await _hRMService.GetUserInfoDetailsAsync(id);
            return Json(vm);
        }
        
        [HttpGet]
        [Route("detail")]
        public async Task<IActionResult> ViewProfile()
        {
            var profile = await _hRMService.GetPersonalProfileAsync();
            if(profile == null)
                ModelState.AddModelError("get_personal_profile", "Failure to get user profile details.");

            return View(profile);
        }


        [HttpPut("user/{id:int}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] EditUserRequest request)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");
            var isSucceed = await _hRMService.EditUserInfoAsync(id, request);
            if (isSucceed) return Ok();

            ModelState.AddModelError("delete_failure", "Failure to delete an account.");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            var isSucceed = await _hRMService.CreateUserAsync(request);
            if (isSucceed) return RedirectToAction("Index");

            ModelState.AddModelError("create_failure", "Failure to create an account.");
            return RedirectToAction("Index");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var isSucceed = await _hRMService.DeleteUserAsync(id);
            if (isSucceed) return Ok();

            ModelState.AddModelError("delete_failure", "Failure to delete an account.");
            return RedirectToAction("Index");
        }
        
        [HttpPut("user/{id:int}/activate")]
        public async Task<IActionResult> ToggleActivate([FromRoute] int id)
        {
            var isSucceed = await _hRMService.ToggleActivateAsync(id);
            if (isSucceed) return Ok();

            ModelState.AddModelError("delete_failure", "Failure to activate an account.");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("role")]
        [ProducesResponseType(typeof(SelectPropertyForCreateAccountResponse), 200)]
        public async Task<IActionResult> GetRoleEnums()
        {
            var allowedRoleForCreateAccount = await _hRMService.GetRolesForCreateAccountAsync();
            return Ok(allowedRoleForCreateAccount);
        }
        
    }
}
