using UnityEngine;

namespace AtosArrows
{
    class AssetHelper
    {
        public static Items.RecipesConfig recipes;
        public static AssetBundle assetBundle;
        public static void Init(string assetbundle, string recipejson)
        {
            assetBundle = Tools.LoadAssetBundle(assetbundle);
            recipes = Tools.LoadJsonFile<Items.RecipesConfig>(recipejson);

            foreach (var recipe in recipes.recipes)
            {
                if (recipe.enabled)
                {
                    if (assetBundle.Contains(recipe.item))
                    {
                        var prefab = assetBundle.LoadAsset<GameObject>(recipe.item);
                        PrefabHelper.AddCustomItem(prefab, recipes);
                        PrefabHelper.AddCustomRecipe(prefab, recipes);
                    }
                }
            }
        }
    }
}
