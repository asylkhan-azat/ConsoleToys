namespace ConsoleToy.Core;

public readonly record struct Point2D(int RowIndex, int ColumnIndex)
{
    public Point2D GetRelative(int row, int column)
    {
        return new Point2D(RowIndex + row, ColumnIndex + column);
    }

    public Point2D Left()
    {
        return this with { ColumnIndex = ColumnIndex - 1 };
    }

    public Point2D LeftTop()
    {
        return new Point2D(RowIndex - 1, ColumnIndex - 1);
    }

    public Point2D LeftBottom()
    {
        return new Point2D(RowIndex + 1, ColumnIndex - 1);
    }

    public Point2D Right()
    {
        return this with { ColumnIndex = ColumnIndex + 1 };
    }

    public Point2D RightTop()
    {
        return new Point2D(RowIndex - 1, ColumnIndex + 1);
    }

    public Point2D RightBottom()
    {
        return new Point2D(RowIndex + 1, ColumnIndex + 1);
    }

    public Point2D Top()
    {
        return this with { RowIndex = RowIndex - 1 };
    }

    public Point2D Bottom()
    {
        return this with { RowIndex = RowIndex + 1 };
    }

    public static implicit operator Point2D((int rowIndex, int columnIndex) indices)
    {
        return new Point2D(indices.rowIndex, indices.columnIndex);
    }
}