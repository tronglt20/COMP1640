﻿using COMP1640.Services;
using COMP1640.ViewModels.AcademicYear;
using COMP1640.ViewModels.AcademicYear.Request;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Utilities.ValidataionAttributes;

namespace COMP1640.Controllers;

[Route("academic-year")]
[COMP1640Authorize(RoleTypeEnum.QAManager)]
public class AcademicYearController : Controller
{
    private readonly AcademicYearService _academicYearService;

    public AcademicYearController(AcademicYearService academicYearService)
    {
        _academicYearService = academicYearService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (TempData.ContainsKey("ErrorMessage"))
        {
            ModelState.AddModelError("error_message", TempData["ErrorMessage"].ToString());
            TempData.Clear();
        }

        var academicYearResponses = await _academicYearService.GetAcademicYearsAsync();
        return View(academicYearResponses.ToList());
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<AcademicYearResponse?> GetById([FromRoute] int id)
    {
        return await _academicYearService.GetAcademicYearById(id);

    }

    [HttpPost]
    public async Task<IActionResult> Create(UpsertAcademicYearRequest request)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");

        var isSuccess = await _academicYearService.CreateAcademicYearAsync(request);

        if (!isSuccess)
        {
            TempData.Add("ErrorMessage", "Failure to create academic year");
        }
        return RedirectToAction("Index");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await _academicYearService.DeleteAcademicYearAsync(id);


        return result.IsLeft ? Ok() : BadRequest(result.Right.ErrorMessage);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Edit([FromRoute] int id, [FromBody] UpsertAcademicYearRequest request)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");

        var isSuccess = await _academicYearService.UpdateAcademicYearAsync(id, request);

        return isSuccess ? Ok() : BadRequest("Failure to update");
    }
}