﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Blue.Private.Win32Imports;
using Blue.Windows;
using GW2PAO.Utility;

namespace GW2PAO.Views
{
    public class OverlayWindow : Window
    {
        /// <summary>
        /// StickyWindow helper object
        /// </summary>
        private StickyWindow stickyWindow;

        public OverlayWindow()
        {
            // For sticky window support
            this.Loaded += OverlayWindowBase_Loaded;
            this.LocationChanged += OverlayWindowBase_LocationChanged;

            // To make a window truely top-most, we have to periodically set the window as top-most using a User32 call
            // So, to do this, we'll create a thread to do it periodically, as long as the window isn't closed
            this.Loaded += (o, e) => Task.Factory.StartNew(this.TopMostThread, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Method that handles making sure the overlay window is always shown
        /// </summary>
        private void TopMostThread()
        {
            while (this.IsVisible)
            {
                Threading.BeginInvokeOnUI(() => User32.SetTopMost(this));
                System.Threading.Thread.Sleep(500);
            }
        }

        private void OverlayWindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.stickyWindow = new StickyWindow(this);
            this.stickyWindow.StickGap = 10;
            this.stickyWindow.StickToScreen = true;
            this.stickyWindow.StickToOther = true;
            this.stickyWindow.StickOnResize = true;
            this.stickyWindow.StickOnMove = true;
        }

        private void OverlayWindowBase_LocationChanged(object sender, EventArgs e)
        {
            System.Windows.Point MousePoint = Mouse.GetPosition(this);
            System.Windows.Point ScreenPoint = this.PointToScreen(MousePoint);

            Win32.SendMessage(this.stickyWindow.Handle, Win32.WM.WM_NCLBUTTONDOWN, Win32.HT.HTCAPTION, Win32.MakeLParam(Convert.ToInt32(ScreenPoint.X), Convert.ToInt32(ScreenPoint.Y)));
            Win32.SendMessage(this.stickyWindow.Handle, Win32.WM.WM_MOUSEMOVE, Win32.HT.HTCAPTION, Win32.MakeLParam(Convert.ToInt32(MousePoint.X), Convert.ToInt32(MousePoint.Y)));
        }
    }
}