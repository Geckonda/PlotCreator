using System.Collections.Generic;

namespace PlotCreator.Domain.Entity
{
    public sealed class DefaultEntityTypeSeed
    {
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int Radius { get; set; }
        public string PropertySchemaJson { get; set; } = "[]";
    }

    public static class DefaultEntityTypes
    {
        public static IReadOnlyList<DefaultEntityTypeSeed> All { get; } = new List<DefaultEntityTypeSeed>
        {
            new()
            {
                Key = "character",
                Label = "Персонажи",
                Color = "#4a2c1a",
                Icon = "⚔",
                Radius = 21,
                PropertySchemaJson = """
                [
                  { "key": "birthday", "label": "Рождение", "kind": "date" },
                  { "key": "deathday", "label": "Смерть", "kind": "date" },
                  { "key": "gender", "label": "Пол", "kind": "text" },
                  { "key": "height", "label": "Рост (см)", "kind": "number" },
                  { "key": "weight", "label": "Вес (кг)", "kind": "number" },
                  { "key": "personality", "label": "Характер", "kind": "textarea" },
                  { "key": "appearance", "label": "Внешность", "kind": "textarea" },
                  { "key": "goals", "label": "Цели", "kind": "textarea" },
                  { "key": "motivation", "label": "Мотивация", "kind": "textarea" },
                  { "key": "conflict", "label": "Конфликт", "kind": "textarea" },
                  { "key": "history", "label": "История", "kind": "textarea" },
                  { "key": "pictureUrl", "label": "Изображение (URL)", "kind": "text" }
                ]
                """
            },
            new()
            {
                Key = "location",
                Label = "Локации",
                Color = "#3d6b4a",
                Icon = "◎",
                Radius = 19,
                PropertySchemaJson = """
                [
                  { "key": "region", "label": "Регион", "kind": "text" },
                  { "key": "climate", "label": "Климат", "kind": "text" },
                  { "key": "pictureUrl", "label": "Изображение (URL)", "kind": "text" }
                ]
                """
            },
            new()
            {
                Key = "event",
                Label = "События",
                Color = "#b8860b",
                Icon = "◆",
                Radius = 17,
                PropertySchemaJson = """
                [
                  { "key": "beginning", "label": "Начало", "kind": "date" },
                  { "key": "ending", "label": "Конец", "kind": "date" },
                  { "key": "chekhovsGun", "label": "Ружьё Чехова", "kind": "checkbox" }
                ]
                """
            },
            new()
            {
                Key = "faction",
                Label = "Фракции",
                Color = "#a02929",
                Icon = "⚜",
                Radius = 19,
                PropertySchemaJson = """
                [
                  { "key": "ideology", "label": "Идеология", "kind": "textarea" },
                  { "key": "headquarters", "label": "Штаб", "kind": "text" }
                ]
                """
            },
            new()
            {
                Key = "episode",
                Label = "Эпизоды",
                Color = "#2c4a7a",
                Icon = "▸",
                Radius = 17,
                PropertySchemaJson = """
                [
                  { "key": "position", "label": "Позиция", "kind": "number" },
                  { "key": "content", "label": "Содержание", "kind": "textarea" }
                ]
                """
            },
            new()
            {
                Key = "artifact",
                Label = "Артефакты",
                Color = "#c45a14",
                Icon = "✦",
                Radius = 15,
                PropertySchemaJson = """
                [
                  { "key": "material", "label": "Материал", "kind": "text" },
                  { "key": "origin", "label": "Происхождение", "kind": "textarea" },
                  { "key": "pictureUrl", "label": "Изображение (URL)", "kind": "text" }
                ]
                """
            },
            new()
            {
                Key = "lore",
                Label = "Лор",
                Color = "#6b3a7a",
                Icon = "◈",
                Radius = 15,
                PropertySchemaJson = """
                [
                  { "key": "era", "label": "Эпоха", "kind": "text" },
                  { "key": "content", "label": "Текст", "kind": "textarea" }
                ]
                """
            }
        };
    }
}
