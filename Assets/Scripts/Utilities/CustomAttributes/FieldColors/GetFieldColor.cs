using UnityEngine;

//Git
namespace Utilities.CustomAttributes.FieldColors
{
    public static class GetFieldColor
    {
        /// <summary>
        /// Conversion de enum vers UnityEngine.Color
        /// </summary>
        /// <param name="color">enum de color predefinies</param>
        /// <returns>UnityEngine.Color</returns>
        public static Color GetFC(FieldColor color) => color switch
        {
            FieldColor.Red => Color.red,
            FieldColor.Orange => Color.Lerp(Color.red, Color.yellow, 0.5f),
            FieldColor.Yellow => Color.yellow,
            FieldColor.Green => Color.green,
            FieldColor.Cyan => Color.cyan,
            FieldColor.Black => Color.black,
            FieldColor.Blue => Color.blue,
            FieldColor.Gray => Color.gray,
            FieldColor.Magenta => Color.magenta,
            FieldColor.White => Color.white,
            FieldColor.Clear => Color.clear,
            _ => Color.red
        };
    }
}