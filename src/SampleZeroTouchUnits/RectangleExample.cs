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
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="length"></param>
    public RectangleExample(double width, double length)
    {
      Length = length;
      Width = width;
      LengthUnit = Unit.ByTypeID($"{meters}-1.0.1");
      WidthUnit = Unit.ByTypeID($"{meters}-1.0.1");
      AreaUnit = Unit.ByTypeID($"{meters2}-1.0.1");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="length"></param>
    /// <param name="widthUnit"></param>
    /// <param name="lengthUnit"></param>
    public RectangleExample(double width, double length, Unit widthUnit, Unit lengthUnit)
    {
      Width = width;
      Length = length;

      LengthUnit = lengthUnit;
      WidthUnit = widthUnit;

      AreaUnit = Unit.ByTypeID($"{meters2}-1.0.1");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetUnit"></param>
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
    /// 
    /// </summary>
    /// <param name="targetUnit"></param>
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
    /// 
    /// </summary>
    /// <param name="targetUnit"></param>
    /// <returns></returns>
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
