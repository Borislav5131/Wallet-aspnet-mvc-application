﻿using Wallet.Core.ViewModels.UserAsset;
using Wallet.Core.ViewModels.Transaction;
using Wallet.Core.ViewModels.User;

namespace Wallet.Core.ViewModels.Home
{
    public class HomeViewModel
    {
        public IEnumerable<UserTransactionsViewModel> UserTransactionsViewModel { get; set; }
        public UserViewModel UserViewModel { get; set; }
        public IEnumerable<UserAssetViewModel>? UserAssetsViewModel { get; set; }
    }
}
