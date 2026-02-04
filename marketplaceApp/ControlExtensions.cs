using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace marketplaceApp
{
    public static class ControlExtensions
    {
        public static void SetGradientBackground(this Control control, Color color1, Color color2,
                                               LinearGradientMode mode = LinearGradientMode.Vertical)
        {
            control.Paint += (sender, e) =>
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    control.ClientRectangle, color1, color2, mode))
                {
                    e.Graphics.FillRectangle(brush, control.ClientRectangle);
                }
            };
        }

        public static void SetGradientBackground(this Control control, Color[] colors,
                                               LinearGradientMode mode = LinearGradientMode.Vertical)
        {
            control.Paint += (sender, e) =>
            {
                if (colors.Length >= 2)
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                        control.ClientRectangle, colors[0], colors[1], mode))
                    {
                        if (colors.Length > 2)
                        {
                            ColorBlend blend = new ColorBlend();
                            blend.Colors = colors;
                            blend.Positions = Enumerable.Range(0, colors.Length)
                                                      .Select(i => (float)i / (colors.Length - 1))
                                                      .ToArray();
                            brush.InterpolationColors = blend;
                        }
                        e.Graphics.FillRectangle(brush, control.ClientRectangle);
                    }
                }
            };
        }
    }
}
