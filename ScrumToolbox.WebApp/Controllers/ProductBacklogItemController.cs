﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumToolbox.ProductBacklogs;
using ScrumToolbox.WebApp.Models.BacklogItems;

namespace ScrumToolbox.WebApp.Controllers
{
    [Route("api/backlogs/{productBacklogId}/pbis")]
    public class ProductBacklogItemController : ControllerBase
    {
        private readonly IProductBacklogContext backlogContext;

        public ProductBacklogItemController(IProductBacklogContext backlogContext)
        {
            this.backlogContext = backlogContext;
        }

        [HttpPost]
        public IActionResult Create(int productBacklogId, [FromBody] BacklogItemCreateDto backlogItemDto)
        {
            return Created("0", null);
        }

        [Route("{backlogItemId}")]
        [HttpGet]
        public IActionResult Get(int productBacklogId, int backlogItemId)
        {
            var productBacklogItem = this.backlogContext
                .BacklogItems
                .Include(pbi => pbi.Tasks)
                .Where(pbi => pbi.ProductBacklogId == productBacklogId)
                .SingleOrDefault(pbi => pbi.Id == backlogItemId);

            if (productBacklogItem == null)
                return BadRequest("Could not find backlog item with specified id and/or backlog id.");

            return Ok(productBacklogItem);
        }

        [HttpGet]
        public IActionResult GetMany(int productBacklogId)
        {
            var pbis = this.backlogContext
                .BacklogItems
                .Include(pbi => pbi.Tasks)
                .Where(pbi => pbi.ProductBacklogId == productBacklogId)
                .ToList();

            return Ok(pbis);
        }

        [Route("{backlogItemId}")]
        [HttpGet]
        public IActionResult Delete(int productBacklogId, int backlogItemId)
        {
            var productBacklogItem = this.backlogContext
                .BacklogItems
                .Where(pbi => pbi.ProductBacklogId == productBacklogId)
                .SingleOrDefault(pbi => pbi.Id == backlogItemId);

            if (productBacklogItem == null)
                return BadRequest("Could not find backlog item with specified id and/or backlog id.");

            this.backlogContext
                .BacklogItems
                .Remove(productBacklogItem);
            this.backlogContext.SaveChanges();

            return NoContent();
        }
    }
}
