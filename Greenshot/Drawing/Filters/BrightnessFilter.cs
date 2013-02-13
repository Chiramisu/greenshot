﻿/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2013  Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on Sourceforge: http://sourceforge.net/projects/greenshot/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Drawing;
using Greenshot.Drawing.Fields;
using Greenshot.Plugin.Drawing;
using GreenshotPlugin.Core;

namespace Greenshot.Drawing.Filters {
	[Serializable()] 
	public class BrightnessFilter : AbstractFilter {
		
		public BrightnessFilter(DrawableContainer parent) : base(parent) {
			AddField(GetType(), FieldType.BRIGHTNESS, 0.9d);
		}

		/// <summary>
		/// Implements the Apply code for the Brightness Filet
		/// </summary>
		/// <param name="graphics"></param>
		/// <param name="applyBitmap"></param>
		/// <param name="rect"></param>
		/// <param name="renderMode"></param>
		public override void Apply(Graphics graphics, Bitmap applyBitmap, Rectangle rect, RenderMode renderMode) {
			Rectangle applyRect = ImageHelper.CreateIntersectRectangle(applyBitmap.Size, rect, Invert);

			if (applyRect.Width == 0 || applyRect.Height == 0) {
				// nothing to do
				return;
			}

			using (BitmapBuffer bbb = new BitmapBuffer(applyBitmap, applyRect)) {
				bbb.Lock();
				double brightness = GetFieldValueAsDouble(FieldType.BRIGHTNESS);
				for (int y = 0; y < bbb.Height; y++) {
					for (int x = 0; x < bbb.Width; x++) {
						if (parent.Contains(applyRect.Left + x, applyRect.Top + y) ^ Invert) {
							Color color = bbb.GetColorAt(x, y);
							int r = Convert.ToInt16(color.R * brightness);
							int g = Convert.ToInt16(color.G * brightness);
							int b = Convert.ToInt16(color.B * brightness);
							r = (r > 255) ? 255 : r;
							g = (g > 255) ? 255 : g;
							b = (b > 255) ? 255 : b;
							bbb.SetColorAt(x, y, Color.FromArgb(color.A, r, g, b));
						}
					}
				}
				bbb.DrawTo(graphics, applyRect.Location);
			}
		}
	}
}
