using Microsoft.AspNetCore.Mvc;
using GameApi.Models;

namespace GameApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private static List<Character> _characters = new();

        [HttpPost]
        public IActionResult CreateCharacter([FromBody] Character character)
        {
            _characters.Add(character);
            return Ok(character);
        }

        [HttpGet]
        public IActionResult GetAllCharacters()
        {
            return Ok(_characters);
        }

        [HttpPost("Attack")]
        public IActionResult NormalAttack([FromQuery] Guid id)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character == null) return NotFound();

            return Ok($"{character.Name} 普通攻擊造成 {(10 + character.Strength)} 點傷害！");
        }

        [HttpPost("Skill")]
        public IActionResult CastSkill([FromQuery] Guid id)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character == null) return NotFound();

            string skillName;
            int damage;

            switch (character.Class)
            {
                case "Mage":
                    skillName = "火球術";
                    damage = character.Intelligence * 2;
                    break;
                case "Assassin":
                    skillName = "剎那";
                    damage = character.Dexterity * 2;
                    break;
                case "Swordman":
                    skillName = "裂地斬";
                    damage = character.Strength * 2;
                    break;
                case "Archer":
                    skillName = "箭雨";
                    damage = character.Dexterity * 2;
                    break;
                default:
                    skillName = "普通技能";
                    damage = 10;
                    break;
            }

            return Ok($"{character.Name} 使用技能「{skillName}」造成 {damage} 點傷害！");
        }

        [HttpPost("Allocate")]
        public IActionResult AllocatePoint(Guid id, string attr, int amount)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character == null) return NotFound();

            switch (attr.ToLower())
            {
                case "str": character.Strength += amount; break;
                case "dex": character.Dexterity += amount; break;
                case "int": character.Intelligence += amount; break;
                default: return BadRequest("無效屬性");
            }

            return Ok($"{character.Name} 增加 {amount} 點 {attr}。");
        }
        [HttpPost("TakeDamage")]
        public IActionResult TakeDamage([FromQuery] Guid id, [FromQuery] int damage)
        {
            var character = _characters.FirstOrDefault(c => c.Id == id);
            if (character == null) return NotFound();

            character.HP -= damage;
            if (character.HP < 0) character.HP = 0;

            return Ok($"{character.Name} 受到 {damage} 點傷害，剩餘 HP：{character.HP}");
        }
    }
}