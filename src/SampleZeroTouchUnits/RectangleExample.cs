using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using System.Collections.Generic;
using System;
using DynamoUnits;
using Dynamo.Graph.Nodes.CustomNodes;

namespace SampleZeroTouchUnits
{
  /// <summary>
  /// The RectangleExample class demonstrates
  /// how to use the Dynamo Units API to convert between units.
  /// </summary>
  public class RectangleExample
  {
    const string meters = "autodesk.unit.unit:meters";  
    const string meters2 = "autodesk.unit.unit:squareMeters";

    /// <summary>
    /// The Length value
    /// </summary>
    private readonly double Length;

    /// <summary>
    /// The Width value
    /// </summary>
    private readonly double Width;

    private Unit LengthUnit;
    private Unit WidthUnit;
    private Unit AreaUnit;

    /// <summary>
    /// Creates an instance of RectangleExample with all units defaulted to metric
    /// </summary>
    /// <param name="width">The width of RectangleExample, defaulted to meters</param>
    /// <param name="length">The length of RectangleExample, defaulted to meters</param>
    public RectangleExample(double width, double length)
    {
      Length = length;
      Width = width;
      LengthUnit = Unit.ByTypeID($"{meters}-1.0.1");
      WidthUnit = Unit.ByTypeID($"{meters}-1.0.1");
      AreaUnit = Unit.ByTypeID($"{meters2}-1.0.1");
    }

    /// <summary>
    /// Creates an instance of RectangleExample with customizable untis
    /// </summary>
    /// <param name="width">The width of RectangleExample</param>
    /// <param name="length">The length of RectangleExample</param>
    /// <param name="widthUnit">The unit for width</param>
    /// <param name="lengthUnit">The unit for length</param>
    public RectangleExample(double width, double length, Unit widthUnit, Unit lengthUnit)
    {
      Width = width;
      Length = length;

      LengthUnit = lengthUnit;
      WidthUnit = widthUnit;

      AreaUnit = Unit.ByTypeID($"{meters2}-1.0.1");
    }

    /// <summary>
    /// Get the length converted to a target unit.
    /// </summary>
    /// <param name="targetUnit">The target unit. Defaults to null</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public double GetLength(Unit targetUnit = null)
    {
      targetUnit ??= LengthUnit;
      ArgumentNullException.ThrowIfNull(targetUnit);

      if (!Unit.AreUnitsConvertible(LengthUnit, targetUnit))
      {
        throw new ArgumentException($"{LengthUnit} is not convertible to {targetUnit}");
      }
 
      var output = Utilities.ConvertByUnits(Length, LengthUnit, targetUnit);
      return output;
    }

    /// <summary>
    /// Get the width converted to a target unit.
    /// </summary>
    /// <param name="targetUnit">The target unit. Defaults to null</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public double GetWidth(Unit targetUnit)
    {
      targetUnit ??= WidthUnit;
      ArgumentNullException.ThrowIfNull(targetUnit);
      if (!Unit.AreUnitsConvertible(WidthUnit, targetUnit))
      {
        throw new ArgumentException($"{LengthUnit} is not convertible to {targetUnit}");
      }

      var output = Utilities.ConvertByUnits(Length, WidthUnit, targetUnit);
      return output;
    }

    string GetFirstSymbolText(Unit unit)
    {
      var symbols = DynamoUnits.Symbol.SymbolsByUnit(unit);
      foreach (var symbol in symbols)
      {
        return symbol.Text;
      }
      return string.Empty;
    }

    /// <summary>
    /// Get the area of the rectangle. Computed as width * length
    /// </summary>
    /// <param name="targetUnit">The target unit for the area value, defaults to null</param>
    /// <returns>A string containing the area value and unit symbol. Ex. "100ft^2"</returns>
    /// <exception cref="ArgumentException"></exception>
    public string GetArea(Unit targetUnit = null)
    {
      targetUnit ??= AreaUnit;
      if (!Unit.AreUnitsConvertible(AreaUnit, targetUnit))
      {
        throw new ArgumentException($"{targetUnit.Name} is not a valid area unit");
      }

      double area = Utilities.ParseExpressionByUnit(targetUnit, $"{Length}{GetFirstSymbolText(LengthUnit)} * {Width}{GetFirstSymbolText(WidthUnit)}");
      return $"{area}{GetFirstSymbolText(targetUnit)}";
    }
  }
}
