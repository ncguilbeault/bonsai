﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bonsai.Design
{
    public class CueBannerTextBox : TextBox
    {
        bool cueBannerVisible;
        Color activeForeColor;

        public string CueBanner { get; set; }

        private void ShowCueBanner()
        {
            if (string.IsNullOrWhiteSpace(Text) && !string.IsNullOrWhiteSpace(CueBanner))
            {
                cueBannerVisible = true;
                activeForeColor = ForeColor;
                ForeColor = Color.Gray;
                Text = CueBanner;
            }
        }

        private void HideCueBanner()
        {
            if (cueBannerVisible)
            {
                cueBannerVisible = false;
                Text = string.Empty;
                ForeColor = activeForeColor;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            if (!Focused) ShowCueBanner();
            base.OnHandleCreated(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            HideCueBanner();
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            ShowCueBanner();
            base.OnLeave(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (!cueBannerVisible)
            {
                base.OnTextChanged(e);
            }
        }
    }
}
