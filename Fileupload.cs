using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnet.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.IO;
using OfficeOpenXml;

namespace dotnet
{
	public class Fileupload
	{
		public List<FileModel> FileUpload(string FilePath)
		{

			IEnumerable<FileModel> items = new FileModel[] { };
			List<FileModel> FileModel = new List<FileModel>
			{

			};

			FileInfo existingFile = new FileInfo(FilePath);
			using (ExcelPackage package = new ExcelPackage(existingFile))
			{
				
				ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
				int colCount = worksheet.Dimension.End.Column;  
				int rowCount = worksheet.Dimension.End.Row;   

				for (int row = 2; row <= rowCount; row++)
				{
					string[] rows = new string[18];
					for (int col = 1; col <= colCount; col++)
					{
						rows[col - 1] = worksheet.Cells[row, col].Value?.ToString().Trim();
					}
					int count = rows.Where(x => x != null).Count();

					if (count == 18)
					{
						FileModel.Add(new FileModel
						{
							Classification = rows[16] == null ? "" : rows[16],
							Description = rows[1] == null ? "" : rows[1],
							Dimension = rows[8] == null ? "" : rows[8],
							Discount = int.Parse(rows[4] == null ? "0" : rows[4]) == 0 ? 0 : int.Parse(rows[4]),
							Images = rows[15].Split(","),
							IsAllowed = rows[10] == null ? false : Boolean.Parse(rows[10]),
							Isdisable = rows[9] == null ? false : Boolean.Parse(rows[9]),
							IsNew = rows[12] == null ? false : Boolean.Parse(rows[12]),
							IsOffer = rows[14] == null ? false : Boolean.Parse(rows[14]),
							IsoutofStock = rows[11] == null ? false : Boolean.Parse(rows[11]),
							IsSale = rows[13] == null ? false : Boolean.Parse(rows[13]),
							Price = int.Parse(rows[2] == null ? "0" : rows[2]) == 0 ? 0 : int.Parse(rows[2]),
							ProductCode = rows[3] == null ? "" : rows[3],
							Quantity = int.Parse(rows[5] == null ? "0" : rows[5]) == 0 ? 0 : int.Parse(rows[5]),
							ShopName = rows[17] == null ? "" : rows[17],
							Title = rows[0] == null ? "" : rows[0],
							Unit = rows[7] == null ? "" : rows[7],
							Weight = float.Parse(rows[6] == null ? "0" : rows[6]) == 0 ? 0 : float.Parse(rows[6]),

						});

					}

				}
			}
			return FileModel;
		}
	}
}