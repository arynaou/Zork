using UnityEngine;
using Zork;

public class ZorkManager : MonoBehaviour
{
    private void Awake()
    {
        TextAsset gameJsonAsset = Resources.Load<TextAsset>(zorkGameFileAssetName);
        Game game = Game.Load(gameJsonAsset.text,outputService,inputService);
        outputService.LocationOutput(game.Player.Location.ToString());
        outputService.ItemOutput(game.Player);
        if (game.Player.Location._Enemy != null)
        { 
            outputService.EnemyOutput(game.Player, game.Player.Location._Enemy.HitPoints, game.Player.Location._Enemy.MaxHitPoints);
        }
    }

    [SerializeField] private string zorkGameFileAssetName = "Game";
    [SerializeField] private UnityOutputService outputService;
    [SerializeField] private UnityInputService inputService;
}
