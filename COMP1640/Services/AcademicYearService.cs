﻿using COMP1640.ViewModels.AcademicYear;
using COMP1640.ViewModels.AcademicYear.Request;
using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Utilities.Types;

namespace COMP1640.Services;

public partial class AcademicYearService
{
    private readonly IIdeaRepository _ideaRepo;
    private readonly IAcademicYearRepository _academicYearRepo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AttachmentService _attachmentService;
    public AcademicYearService(IAcademicYearRepository academicYearRepository
        , IUnitOfWork unitOfWork
        , IIdeaRepository ideaRepo
        , AttachmentService attachmentService)
    {
        _academicYearRepo = academicYearRepository;
        _unitOfWork = unitOfWork;
        _ideaRepo = ideaRepo;
        _attachmentService = attachmentService;
    }

    public async Task<AcademicYearResponse?> GetAcademicYearById(int id)
    {
        return await _academicYearRepo.GetQuery(a => a.Id == id)
            .Select(new AcademicYearResponse().GetSelection())
            .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<AcademicYearResponse>> GetAcademicYearsAsync()
    {
        var academicYearResponses = await _academicYearRepo.GetAllQuery()
            .OrderByDescending(y => y.EndDate)
            .Select(new AcademicYearResponse().GetSelection())
            .ToListAsync();

        return academicYearResponses;
    }

    public async Task<bool> CreateAcademicYearAsync(UpsertAcademicYearRequest request)
    {
        var isValid = await IsGreaterThanLatestAcademicYearAsync(request.ClosureDate);

        if (!isValid) return false;

        var academicYear =
            new AcademicYear(request.Name, request.ClosureDate, request.FinalClosureDate, request.EndDate);

        await _academicYearRepo.InsertAsync(academicYear);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAcademicYearAsync(int academicYearId, UpsertAcademicYearRequest request)
    {
        // TODO: Clarify requirement, implement validation when update academic year.
        var existedAcademicYear = await _academicYearRepo.GetAsync(a => a.Id == academicYearId);
        if (existedAcademicYear == null) return false;

        existedAcademicYear.UpdateAcademicYear(request.Name, request.ClosureDate, request.FinalClosureDate, request.EndDate);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<Either<bool, Failure>> DeleteAcademicYearAsync(int id)
    {
        var existedAcademicYear = await _academicYearRepo.GetAsync(a => a.Id == id);
        if (existedAcademicYear == null) return new Either<bool, Failure>(new Failure("Not found academic year"));

        if (await IsHasAnyIdea(id))
        {
            return new Either<bool, Failure>(
                new Failure("Can not delete this academic year. There are ideas in this year."));
        }

        await _academicYearRepo.DeleteAsync(existedAcademicYear);

        await _unitOfWork.SaveChangesAsync();
        return new Either<bool, Failure>(true);
    }

    private async Task<bool> IsGreaterThanLatestAcademicYearAsync(DateTime requestClosureDate)
    {
        var latestAcademicYear = await _academicYearRepo.GetLatestAcademicYearAsync();
        if (latestAcademicYear == null) return true;
        return requestClosureDate > latestAcademicYear.EndDate;
    }

    private async Task<bool> IsHasAnyIdea(int academicId)
    {
        return await _academicYearRepo.AnyAsync(_ => _.Ideas.Any());
    }
}