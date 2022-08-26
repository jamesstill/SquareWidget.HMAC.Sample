using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SampleApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetsController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            await Task.Delay(10);
            var item = MockDatabase.Widgets.FirstOrDefault(w => w.ID == id);
            return Ok(item);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(10);
            var items = MockDatabase.Widgets.OrderBy(w => w.Name);
            return Ok(items);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Page([FromQuery] int pageSize = 20, [FromQuery] int pageIndex = 0)
        {
            await Task.Delay(10);
            var items = MockDatabase.Widgets
                .OrderBy(w => w.Name)
                .Skip(pageSize * pageIndex)
                .Take(pageSize).ToList();

            return Ok(items);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Widget item)
        {
            await Task.Delay(10);
            var widget = MockDatabase.Widgets.FirstOrDefault(w => w.ID == item.ID);
            if (widget == null)
            {
                return NotFound();
            }

            widget.Name = item.Name;
            widget.Shape = item.Shape;
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Widget item)
        {
            await Task.Delay(10);
            item.ID = MockDatabase.UniqueWidgetId;
            MockDatabase.UniqueWidgetId++;
            MockDatabase.Widgets.Add(item);
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Task.Delay(10);
            var widget = MockDatabase.Widgets.FirstOrDefault(w => w.ID == id);
            MockDatabase.Widgets.Remove(widget);
            return Ok();
        }
    }

    public static class MockDatabase
    {
        public static IList<Widget> Widgets { get; }
        public static int UniqueWidgetId = 5;

        static MockDatabase()
        {
            Widgets = new List<Widget>()
            {
                new Widget { ID = 1, Name = "Cog", Shape = "Square" },
                new Widget { ID = 2, Name = "Gear", Shape = "Round" },
                new Widget { ID = 3, Name = "Sprocket", Shape = "Octagonal" },
                new Widget { ID = 4, Name = "Pinion", Shape = "Triangular" }
            };
        }
    }
}
