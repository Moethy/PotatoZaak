using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PotatoZaak.Data;
using PotatoZaak.Models;

namespace PotatoZaak.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var viewModel = new OrderProductViewModel
            {
                Products = _context.Products.ToList(),
                Orders = _context.Orders.ToList(),
            };
            return View(viewModel);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // GET: Orders/Create
        public IActionResult Create()
        {
            var availableProducts = _context.Products.ToList();

            var viewModel = new OrderProductViewModel
            {
                Products = _context.Products.ToList(),
                Orders = _context.Orders.ToList(),
                OrderProducts = new List<OrderProduct>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderProductViewModel model, IEnumerable<OrderProduct> selectedProducts)
        {
            if (ModelState.IsValid)
            {
                // Maak een nieuw Order-object aan
                var order = new Order
                {
                    DateTime = DateTime.Now
                };

                // Voeg het order toe aan de database
                _context.Orders.Add(order);
                _context.SaveChanges();

                // Voeg de geselecteerde producten aan het order toe
                foreach (var productSelection in selectedProducts)
                {
                    var product = _context.Products.Find(productSelection.ProductId);
                    if (product != null)
                    {
                        var orderProductEntity = new OrderProduct
                        {
                            OrderId = order.Id,
                            ProductId = productSelection.ProductId,
                            Quantity = productSelection.Quantity
                        };

                        // Voeg het orderproduct toe aan de database
                        _context.OrderProducts.Add(orderProductEntity);
                    }
                }

                // Sla wijzigingen op in de database
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }



        // Andere acties en methoden zoals Edit en Delete blijven ongewijzigd

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateTime,OrderNr")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders' is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
