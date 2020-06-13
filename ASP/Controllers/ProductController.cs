using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using ASP.DAL.Entities;
using ASP.Extensions;
using ASP.Models;
using Microsoft.Extensions.Logging;

namespace ASP.Controllers
{
    public class ProductController : Controller
    {
        private ILogger _logger;
        ApplicationDbContext _context;


        //public List<Boots> _boots;
        //private List<BootsGroup> _bootsGroups;
        int _pageSize;


        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _pageSize = 3;
            _context = context;
          //  _logger = logger;
        }


        [Route("Catalog")]
        [Route("Catalog/Page_{pageNo}")]
        public IActionResult Index(int? group, int pageNo = 1)
        {
            //var items = _context.Bootses.Skip((pageNo - 1) * _pageSize).Take(_pageSize).ToList();

            var groupName = group.HasValue ? _context.BootsGroups.Find(group.Value)?.GroupName : "all groups";

          //  var text = $"info: group={groupName},  page={pageNo}";
          //  _logger.LogInformation(text);




           // var groupMame = group.HasValue ? _context.BootsGroups.Find(group.Value)?.GroupName : "all groups";
          //  _logger.LogInformation($"info: group={groupMame},  page={pageNo}");







            var bootsFiltered = _context.Bootses.Where(d => !group.HasValue
                                                   || d.BootsId == group.Value).ToList();

            // Поместить список групп во ViewData 
            ViewData["Groups"] = _context.BootsGroups; 

            // Получить id текущей группы и поместить в TempData
            var currentGroup = 0;
            try
            {
                int.TryParse(HttpContext.Request.Query["group"], out currentGroup);
                TempData["CurrentGroup"] = currentGroup;
            }
            catch (NullReferenceException e)
            {

                currentGroup = 0;
            }

            if (Request.IsAjaxRequest()) 
                return PartialView("_ListPartial", ListViewModel<Boots>.GetModel(bootsFiltered, pageNo, _pageSize));
            return View(ListViewModel<Boots>.GetModel(bootsFiltered, pageNo, _pageSize));

            //if (Request.Headers["x-requested-with"] == "XMLHttpRequest")
            //    return PartialView("_ListPartial", ListViewModel<Boots>.GetModel(bootsFiltered, pageNo, _pageSize));
            //return View(ListViewModel<Boots>.GetModel(bootsFiltered, pageNo, _pageSize));

            // return View(ListViewModel<Boots>.GetModel(bootsFiltered, pageNo, _pageSize));
        }






    }
}
