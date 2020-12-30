using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
namespace dotnet.Controllers {
	[Route ("api/[controller]")]
	[ApiController]
	public class FileuploadController : ControllerBase {

		private readonly Context _db;
		Fileupload fileupload = new Fileupload ();
		public FileuploadController (Context context) {
			_db = context;
		}

		[HttpPost]
		public async Task<ActionResult> Post ([FromBody] String FilePath) {
			try {
				Console.WriteLine(FilePath);
				var pathToSave = Path.Combine (Directory.GetCurrentDirectory (), FilePath);
				var response = fileupload.FileUpload (pathToSave);

				if (response == null || response.Count <= 0) {
					return NotFound ();
				}
				for (int i = 0; i < response.Count; i++) {
					var Product = _db.Products.Where (x => x.Title.ToLower ().Trim () == response[i].Title.ToLower ().Trim ()).FirstOrDefault ();
					var classification = _db.Classifications.Where (x => x.Name.ToLower ().Trim () == response[i].Classification.ToLower ().Trim ()).FirstOrDefault ();
					var Shop = _db.Shops.Where (x => x.Name.ToLower ().Trim () == response[i].ShopName.ToLower ().Trim ()).FirstOrDefault ();
					if (classification != null && Shop != null) {
						Console.WriteLine("FOUND");
						Product obj = new Product () {
							    ClassificationId = classification.Id,
								Description = response[i].Description,
								Dimension = response[i].Dimension,
								Discount = response[i].Discount,
								IsAllowed = response[i].IsAllowed,
								IsDisabled = response[i].Isdisable,
								IsNew = response[i].IsNew,
								IsOffer = response[i].IsOffer,
								IsOutOfStock = response[i].IsoutofStock,
								IsSale = response[i].IsSale,
								Price = response[i].Price,
								ProductCode = response[i].ProductCode,
								ProductDetail = response[i].Description,
								Quantity = response[i].Quantity,
								ShopId = Shop.Id,
								Title = response[i].Title,
								Unit = response[i].Unit,
								Weight = response[i].Weight
						};

						if (Product != null) {
							Product.Description = response[i].Description;
							Product.Dimension = response[i].Dimension;
							Product.Discount = response[i].Discount;
							Product.IsAllowed = response[i].IsAllowed;
							Product.IsDisabled = response[i].Isdisable;
							Product.IsNew = response[i].IsNew;
							Product.IsOffer = response[i].IsOffer;
							Product.IsOutOfStock = response[i].IsoutofStock;
							Product.IsSale = response[i].IsSale;
							Product.Price = response[i].Price;
							Product.ProductCode = response[i].ProductCode;
							Product.ProductDetail = response[i].Description;
							Product.Quantity = Product.Quantity + response[i].Quantity;
							// ShopId = Shop.Id,
							Product.Title = response[i].Title;
							Product.Unit = response[i].Unit;
							Product.Weight = response[i].Weight;

							_db.Entry (Product).State = EntityState.Modified;
							await _db.SaveChangesAsync ();
						} else {
							_db.Products.Update (obj);

							await _db.SaveChangesAsync ();

						}

						for (int x = 0; x < response[i].Images.Length; x++) {
							Image ImageObj = new Image () {
								ProductId = Product != null ? Product.Id : obj.Id,
									Path = response[i].Images[x]

							};
							_db.Images.Update (ImageObj);

							await _db.SaveChangesAsync ();
						}

					} else {
						return NotFound ();
					}
				}
			} catch {
				return NotFound ();
			}

			return Ok ();
		}

	}
}