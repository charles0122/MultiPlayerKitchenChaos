using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {
    // [SerializeField] private KitchenObjectSO cutKitchenObjectSO;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                // 玩家拿着还没切的物品 放到切菜台上
                player.GetKitchenObject().SetkitchenObjectParent(this);
                cuttingProgress = 0;
            }
        } else {
            if (player.HasKitchenObject()) {

            } else {
                // 将切菜台的物品放回玩家手中
                GetKitchenObject().SetkitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player) {
        base.InteractAlternate(player);
        if (HasKitchenObject()) {
            // 切菜进度
            cuttingProgress++;
            // 找到切菜菜谱
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            if (cuttingProgress>=cuttingRecipeSO.cuttingProgressMax) {
                // 根据菜谱找到生成物品
                KitchenObjectSO output = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                // 自我销毁
                GetKitchenObject().DestroySelf();
                // 生成切菜物品
                KitchenObject.SpawnKitchenObject(output, this);
            }


            ;
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(input);
        return cuttingRecipeSO != null ? cuttingRecipeSO.output : null ;
    }

    private bool HasRecipeWithInput(KitchenObjectSO input) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(input);
        return cuttingRecipeSO != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO input) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == input) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}