using ERPSystem.Application.DTOs.Branch;
using ERPSystem.Application.Services.BranchManagement;
using Helper.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        [Authorize(Policy = "Branch.View")]
        public async Task<IActionResult> GetAllBranches()
        {
            var result = await _branchService.GetAllBranchesAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{Id:int}")]
        [Authorize(Policy = "Branch.View")]
        public async Task<IActionResult> GetBranchById([FromRoute] int Id)
        {
            var result = await _branchService.GetBranchByIdAsync(Id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Policy = "Branch.Add")]
        public async Task<IActionResult> AddBranch([FromBody] BranchDTO branchDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }

            var result = await _branchService.AddBranchAsync(branchDTO);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{Id:int}")]
        [Authorize(Policy = "Branch.Edit")]
        public async Task<IActionResult> EditBranch([FromRoute] int Id, [FromBody] BranchDTO branchDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Errors = ModelState });
            }

            var result = await _branchService.UpdateBranchAsync(branchDTO, Id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(Policy = "Branch.Delete")]
        public async Task<IActionResult> DeleteBranch([FromRoute] int Id)
        {
            var result = await _branchService.DeleteBranchAsync(Id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
