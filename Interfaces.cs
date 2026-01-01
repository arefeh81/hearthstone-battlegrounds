using System;
using System.Collections.Generic;

namespace HB.UI
{
    /// <summary>
    /// UI -> Core command sink. Core should serialize/route this to mock server or simulation.
    /// </summary>
    public interface ICommandSender
    {
        void Send(ActionCommand cmd);
    }

    /// <summary>
    /// Core -> UI event stream. UI may poll events per frame (core can enforce N=5).
    /// </summary>
    public interface IEventSource
    {
        bool TryDequeue(out ServerEvent evt);
    }

    [Serializable]
    public struct ActionCommand
    {
        public string action; // BUY, PLAY, MOVE, SELL, FREEZE, REFRESH, END_TURN, DISCOVER_CHOICE, CHOOSE_ONE
        public int? shop_slot;
        public int? source_hand_slot;
        public int? source_board_slot;
        public int? target_board_slot;
        public string choice_uuid;
        public int? option_index;

        public override string ToString()
        {
            return $"ActionCommand(action={action}, shop={shop_slot}, hand={source_hand_slot}, board={source_board_slot}, target={target_board_slot}, opt={option_index})";
        }
    }

    [Serializable]
    public class CardDTO
    {
        public string instance_id = "";
        public string card_id = "";
        public string name = "";
        public int attack;
        public int health;
        public List<string> keywords = new List<string>();
        public int tier = 1;
        public bool golden = false;
    }

    public enum ZoneType { Shop, Hand, Board }

    [Serializable]
    public class ZoneRef
    {
        public ZoneType zone;
        public int index; // slot index
    }

    [Serializable]
    public class ServerEvent
    {
        public string type = "";      // "delta_state", "combat_event", ...
        public string event_uuid = "";
        public int step = 0;
        public string kind = "";      // payload.kind
        public string log = "";       // payload.log or synthesized
        public object payload = null; // typed payload optional
    }

    /// <summary>
    /// Snapshot for Recruit UI rendering.
    /// </summary>
    [Serializable]
    public class RecruitStateDTO
    {
        public int gold;
        public bool shop_frozen;
        public List<CardDTO> shop = new List<CardDTO>();     // size 3..5 depending on tier
        public List<CardDTO> hand = new List<CardDTO>();     // up to 10
        public List<CardDTO> board = new List<CardDTO>();    // size 7, use null entries for empty slots
        public int tavern_tier;
    }
}
