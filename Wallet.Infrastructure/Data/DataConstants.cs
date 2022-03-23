namespace Wallet.Infrastructure.Data
{
    public class DataConstants
    {
        public class User
        {
            public const double UserBalanceMaxValue = double.MaxValue;
            public const double UserBalanceMinValue = 0;
            public const int UserMaxImageSize = 2 * 1024 * 1024;
        }
        
        public class Asset
        {
            public const int AssetMaxNameLenght = 30;
            public const int AssetMinNameLenght = 3;
            public const int AssetMaxAbbreviationLenght = 10;
            public const int AssetMinAbbreviationLenght = 2;
            public const double AssetMaxValue = 100000;
            public const double AssetMinValue = 0;
            public const double AssetMaxAmount = 100000;
            public const double AssetMinAmount = 10;
            public const double AssetMaxQuantity = 1000;
            public const double AssetMinQuantity = 0;
            public const int AssetMaxLogoSize = 2 * 1024 * 1024;
        }

        public class UserAsset
        {
            public const int UserAssetMaxNameLenght = 30;
            public const int UserAssetMinNameLenght = 3;
            public const int UserAssetMaxAbbreviationLenght = 10;
            public const int UserAssetMinAbbreviationLenght = 2;
            public const double UserAssetMaxValue = 100000;
            public const double UserAssetMinValue = 0;
            public const double UserAssetMaxAmount = 100000;
            public const double UserAssetMinAmount = 10;
            public const double UserAssetMaxQuantity = 1000;
            public const double UserAssetMinQuantity = 0;
            public const int UserAssetCategoryMaxNameLenght = 30;
            public const int UserAssetCategoryMinNameLenght = 3;
        }
        
        public class Category
        {
            public const int CategoryMaxNameLenght = 30;
            public const int CategoryMinNameLenght = 3;
            public const int CategoryDescriptionMaxLenght = 500;
            public const int CategoryDescriptionMinLenght = 0;
        }
       
        public class Wallet
        {
            
        }

        public class Transaction
        {
            public const double TransactionMaxValue = 100000;
            public const double TransactionMinValue = 1;
            public const double TransactionMaxAmount = 100000;
            public const double TransactionMinAmount = 0;
        }
    }
}
