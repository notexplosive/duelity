using Machina.Data;
using Machina.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duel.Data
{
    public class PlayerAnimations
    {
        public IFrameAnimation Idle { get; }
        public IFrameAnimation Move { get; }
        public IFrameAnimation Act { get; }

        public PlayerAnimations(string idleAssetName, string moveAssetName, string actAssetName)
        {

            Idle = MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>(idleAssetName);
            Move = MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>(moveAssetName);
            Act = MachinaClient.Assets.GetMachinaAsset<IFrameAnimation>(actAssetName);
        }

        public static readonly PlayerAnimations Ernesto = new PlayerAnimations("ernesto-idle", "ernesto-move", "ernesto-move");
        public static readonly PlayerAnimations Miranda = new PlayerAnimations("miranda-idle", "miranda-move", "miranda-move");
        public static readonly PlayerAnimations Steven = new PlayerAnimations("steven-idle", "steven-move", "steven-move");
        // There is no Bennigan PlayerAnimation because he's different
    }
}
