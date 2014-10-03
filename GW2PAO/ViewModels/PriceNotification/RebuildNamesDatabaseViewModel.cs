﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GW2PAO.API.Services.Interfaces;
using GW2PAO.PresentationCore;
using GW2PAO.Utility;
using NLog;

namespace GW2PAO.ViewModels.PriceNotification
{
    public class RebuildNamesDatabaseViewModel : NotifyPropertyChangedBase
    {
        /// <summary>
        /// Default logger
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private int progress;
        private int totalRequests;
        private bool isComplete;

        /// <summary>
        /// The Commerce Service
        /// </summary>
        private ICommerceService commerceService;

        /// <summary>
        /// Cancellation token used for cancelling the operation
        /// </summary>
        private CancellationTokenSource cancelToken;

        /// <summary>
        /// The current progress of the rebuild operation
        /// </summary>
        public int Progress
        {
            get { return this.progress; }
            set { this.SetField(ref this.progress, value); }
        }

        /// <summary>
        /// The total number of requests that will be performed for the rebuild
        /// </summary>
        public int TotalRequests
        {
            get { return this.totalRequests; }
            set { this.SetField(ref this.totalRequests, value); }
        }

        /// <summary>
        /// True of the rebuild is complete, else false
        /// </summary>
        public bool IsComplete
        {
            get { return this.isComplete; }
            set { this.SetField(ref this.isComplete, value); }
        }

        /// <summary>
        /// Removes the price watch
        /// </summary>
        public DelegateCommand CancelCommand { get { return new DelegateCommand(this.Cancel); } }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="commerceService">The commerce service</param>
        public RebuildNamesDatabaseViewModel(ICommerceService commerceService)
        {
            this.commerceService = commerceService;
            this.cancelToken = new CancellationTokenSource();
            this.Progress = 0;

            logger.Debug("Starting rebuild of item database");
            this.TotalRequests = this.commerceService.ItemsDatabaseBuilder.RebuildItemDatabase(this.HandleIncrementProgress, this.HandleComplete, this.cancelToken.Token);
        }

        /// <summary>
        /// Increments the Progress member
        /// </summary>
        private void HandleIncrementProgress()
        {
            logger.Debug("Incrementing progress - progress={0}", this.progress);
            Threading.BeginInvokeOnUI(() => this.Progress++);
        }

        /// <summary>
        /// Handles the rebuild complete action
        /// </summary>
        private void HandleComplete()
        {
            logger.Debug("Rebuild completed");
            Threading.BeginInvokeOnUI(() =>
                {
                    this.Progress = this.TotalRequests;
                    this.IsComplete = true;
                });

            // Have the service reload the new database
            this.commerceService.ReloadDatabase();
        }

        /// <summary>
        /// Cancels the operation
        /// </summary>
        private void Cancel()
        {
            logger.Debug("Cancelling");
            this.cancelToken.Cancel();
        }
    }
}
