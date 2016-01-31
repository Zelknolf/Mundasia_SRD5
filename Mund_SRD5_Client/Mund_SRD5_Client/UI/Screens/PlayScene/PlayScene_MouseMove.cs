using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

using Mundasia.Objects;

namespace Mundasia.Interface
{
    public partial class PlayScene : Panel
    {
        /// <summary>
        /// Manages the calculation of mouse overs, and predicts click targets.
        /// </summary>
        private void PlayScene_MouseMove(object sender, MouseEventArgs e)
        {
            IPlaySceneDrawable target = PlaySceneDrawable.GetTarget(e.Location, drawableImages);
            if (currentMouseOver != null)
            {
                this.Invalidate(new Rectangle(currentMouseOver.GetImageLocation(), currentMouseOver.GetTemplateImage().Size));
                currentMouseOver.SetMousedOver(false);
            }
            if (target != null)
            {
                this.Invalidate(new Rectangle(target.GetImageLocation(), target.GetTemplateImage().Size));
                target.SetMousedOver(true);
            }
            currentMouseOver = target;
            this.Update();
        }
    }
}
