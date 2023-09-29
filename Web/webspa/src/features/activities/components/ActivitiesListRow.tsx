import { TableCell } from "@mui/material";
import { ITableColumns } from "../../../types/TableColumns";

interface ActivitiesListRowProp {
    column: ITableColumns;
    value: string | number;
}
export const ActivitiesListRow = (prop: ActivitiesListRowProp) => {

    return (<TableCell key={prop.column.id} align={prop.column.align}>
        {prop.column.format && typeof prop.value === 'number'
            ? prop.column.format(prop.value)
            : prop.value}
    </TableCell>)

}