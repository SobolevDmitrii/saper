using System;
using System.Collections.Generic;
using System.Linq;
using Saper.GameUnits;

namespace Saper.Field;

public class BackGroundField
{
    private int _xField = 10;
    private int _yField = 10;
    private int group = 0;
    private int indexGroup = 1;

    public Unit[,] CreateGameField(int xField = default, int yField = default)
    {
        if (xField == 0 || yField == 0)
        {
            xField = _xField;
            yField = _yField;
        }

        var listBombs = CreatBombs(xField, yField);
        var index = 0;
        Unit[,] gameField = new Unit[xField, yField];
        for (var i = 0; i < xField; i++)
        {
            for (var j = 0; j < yField; j++)
            {
                index++;
                if (listBombs != null && listBombs.Any(q => q == index))
                {
                    gameField[i, j] = new Unit(true);
                }
                else
                {
                    gameField[i, j] = new Unit(false);
                }
            }
        }

        for (var i = 0; i < xField; i++)
        {
            for (var j = 0; j < yField; j++)
            {
                if (gameField[i, j]._bomb)
                {
                    gameField[i, j].countBombs = 10;
                }
                else
                {
                    if (i - 1 >= 0 && j - 1 >= 0)
                        if (gameField[i - 1, j - 1]._bomb)
                            gameField[i, j].countBombs++;
                    if (j - 1 >= 0)
                        if (gameField[i, j - 1]._bomb)
                            gameField[i, j].countBombs++;
                    if (i + 1 < xField && j - 1 >= 0)
                        if (gameField[i + 1, j - 1]._bomb)
                            gameField[i, j].countBombs++;
                    if (i + 1 < xField)
                        if (gameField[i + 1, j]._bomb)
                            gameField[i, j].countBombs++;
                    if (i + 1 < xField && j + 1 < yField)
                        if (gameField[i + 1, j + 1]._bomb)
                            gameField[i, j].countBombs++;
                    if (j + 1 < yField)
                        if (gameField[i, j + 1]._bomb)
                            gameField[i, j].countBombs++;
                    if (i - 1 >= 0 && j + 1 < yField)
                        if (gameField[i - 1, j + 1]._bomb)
                            gameField[i, j].countBombs++;
                    if (i - 1 >= 0)
                        if (gameField[i - 1, j]._bomb)
                            gameField[i, j].countBombs++;
                }
            }
        }

        for (var i = 0; i < xField; i++)
        {
            for (var j = 0; j < yField; j++)
            {
                if (gameField[i, j].countBombs == 0)
                {
                    var currentGroup = 0;
                    if (i - 1 >= 0 && j - 1 >= 0 && gameField[i - 1, j - 1].group.Any() &&
                        gameField[i - 1, j - 1].countBombs == 0)
                        currentGroup = gameField[i - 1, j - 1].group.First();
                    if (j - 1 >= 0 && gameField[i, j - 1].group.Any() && gameField[i, j - 1].countBombs == 0)
                        currentGroup = gameField[i, j - 1].group.First();
                    if (i + 1 < xField && j - 1 >= 0 && gameField[i + 1, j - 1].group.Any() &&
                        gameField[i + 1, j - 1].countBombs == 0)
                        currentGroup = gameField[i + 1, j - 1].group.First();
                    if (i + 1 < xField && gameField[i + 1, j].group.Any() && gameField[i + 1, j].countBombs == 0)
                        currentGroup = gameField[i + 1, j].group.First();
                    if (i + 1 < xField && j + 1 < yField && gameField[i + 1, j + 1].group.Any() &&
                        gameField[i + 1, j + 1].countBombs == 0)
                        currentGroup = gameField[i + 1, j + 1].group.First();
                    if (j + 1 < yField && gameField[i, j + 1].group.Any() && gameField[i, j + 1].countBombs == 0)
                        currentGroup = gameField[i, j + 1].group.First();
                    if (i - 1 >= 0 && j + 1 < yField && gameField[i - 1, j + 1].group.Any() &&
                        gameField[i - 1, j + 1].countBombs == 0)
                        currentGroup = gameField[i - 1, j + 1].group.First();
                    if (i - 1 >= 0 && gameField[i - 1, j].group.Any() && gameField[i - 1, j].countBombs == 0)
                        currentGroup = gameField[i - 1, j].group.First();
                    if (currentGroup != 0 || currentGroup== indexGroup)
                    {
                        if (!gameField[i, j].group.Contains(currentGroup))gameField[i, j].group.Add(currentGroup);
                        if (i - 1 >= 0 && j - 1 >= 0 && !gameField[i - 1, j - 1].group.Contains(currentGroup) && gameField[i - 1, j - 1].countBombs != 0) gameField[i - 1, j - 1].group.Add(currentGroup);
                        if (j - 1 >= 0 && !gameField[i, j - 1].group.Contains(currentGroup)) gameField[i, j - 1].group.Add(currentGroup);
                        if (i + 1 < xField && j - 1 >= 0 && !gameField[i + 1, j - 1].group.Contains(currentGroup) && gameField[i + 1, j - 1].countBombs !=0) gameField[i + 1, j - 1].group.Add(currentGroup);
                        if (i + 1 < xField && !gameField[i + 1, j].group.Contains(currentGroup)) gameField[i + 1, j].group.Add(currentGroup);
                        if (i + 1 < xField && j + 1 < yField && !gameField[i + 1, j + 1].group.Contains(currentGroup) && gameField[i + 1, j + 1].countBombs !=0) gameField[i + 1, j + 1].group.Add(currentGroup);
                        if (j + 1 < yField && !gameField[i, j + 1].group.Contains(currentGroup)) gameField[i, j + 1].group.Add(currentGroup);
                        if (i - 1 >= 0 && j + 1 < yField && !gameField[i - 1, j + 1].group.Contains(currentGroup) && gameField[i - 1, j + 1].countBombs !=0) gameField[i - 1, j + 1].group.Add(currentGroup);
                        if (i - 1 >= 0 && !gameField[i - 1, j].group.Contains(currentGroup)) gameField[i - 1, j].group.Add(currentGroup);
                    }
                    else
                    {
                        indexGroup++;
                        if (!gameField[i, j].group.Contains(indexGroup))gameField[i, j].group.Add(indexGroup);
                        if (i - 1 >= 0 && j - 1 >= 0 && !gameField[i - 1, j - 1].group.Contains(indexGroup) && gameField[i - 1, j - 1].countBombs != 0) gameField[i - 1, j - 1].group.Add(indexGroup);
                        if (j - 1 >= 0 && !gameField[i, j - 1].group.Contains(indexGroup)) gameField[i, j - 1].group.Add(indexGroup);
                        if (i + 1 < xField && j - 1 >= 0 && !gameField[i + 1, j - 1].group.Contains(indexGroup) && gameField[i + 1, j - 1].countBombs !=0) gameField[i + 1, j - 1].group.Add(indexGroup);
                        if (i + 1 < xField && !gameField[i + 1, j].group.Contains(indexGroup)) gameField[i + 1, j].group.Add(indexGroup);
                        if (i + 1 < xField && j + 1 < yField && !gameField[i + 1, j + 1].group.Contains(indexGroup) && gameField[i + 1, j + 1].countBombs !=0) gameField[i + 1, j + 1].group.Add(indexGroup);
                        if (j + 1 < yField && !gameField[i, j + 1].group.Contains(indexGroup)) gameField[i, j + 1].group.Add(indexGroup);
                        if (i - 1 >= 0 && j + 1 < yField && !gameField[i - 1, j + 1].group.Contains(indexGroup) && gameField[i - 1, j + 1].countBombs !=0) gameField[i - 1, j + 1].group.Add(indexGroup);
                        if (i - 1 >= 0 && !gameField[i - 1, j].group.Contains(indexGroup)) gameField[i - 1, j].group.Add(indexGroup);
                    }
                }
            }
        }

        return gameField;
    }

    private static List<int>? CreatBombs(int xField, int yField)
    {
        var countBombs = (uint) (xField * yField * 0.1);
        var rand = new Random();
        List<int> listIndexBombs = new List<int>();
        while (countBombs > 0)
        {
            var random = rand.Next(1, (xField * yField));
            if (listIndexBombs is {Count: > 0})
            {
                if (listIndexBombs.All(q => q != random))
                {
                    listIndexBombs.Add(random);
                    countBombs--;
                }
            }
            else
            {
                listIndexBombs.Add(random);
                countBombs--;
            }
        }

        return listIndexBombs;
    }
}