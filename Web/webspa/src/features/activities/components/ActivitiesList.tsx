import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow } from "@mui/material";
import React from "react";
import useFetchActivities from "../hooks/FetchActivities";
import { ActivitiesListRow } from "./ActivitiesListRow";
import { TableColumns } from "../../../types/TableColumns";

export const ActivitiesList = () => {

    const columns: readonly TableColumns[] = [
        {
            id: 'id', label: 'Identifiant', minWidth: 100, align: 'right',
            format: (value: number) => value.toLocaleString('en-US'),
        },
        { id: 'name', label: 'Nom', minWidth: 170 },
        {
            id: 'athlete',
            label: 'Athlete',
            minWidth: 170
        },
        {
            id: 'location',
            label: 'Lieu',
            minWidth: 170
        }
    ];

    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(10);

    const handleChangePage = (_event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(+event.target.value);
        setPage(0);
    };

    const { rows } = useFetchActivities("");

    return (
        <Paper sx={{ width: '100%', overflow: 'hidden' }}>
            <TableContainer sx={{ maxHeight: 440 }}>
                <Table stickyHeader aria-label="sticky table">
                    <TableHead>
                        <TableRow>
                            {columns.map((column) => (
                                <TableCell
                                    key={column.id}
                                    align={column.align}
                                    style={{ minWidth: column.minWidth }}
                                >
                                    {column.label}
                                </TableCell>
                            ))}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {rows
                            .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                            .map((row, index) => {
                                return (
                                    <TableRow hover role="checkbox" tabIndex={-1} key={row.id}>
                                        {columns.map((column) => {
                                            const value = row[column.id];
                                            return (
                                                <ActivitiesListRow
                                                    key={index + "_" + column.id}
                                                    column={column}
                                                    value={value}
                                                ></ActivitiesListRow>
                                            );
                                        })}
                                    </TableRow>
                                );
                            })}
                    </TableBody>
                </Table>
            </TableContainer>
            <TablePagination
                rowsPerPageOptions={[10, 25, 100]}
                component="div"
                count={rows.length}
                rowsPerPage={rowsPerPage}
                page={page}
                onPageChange={handleChangePage}
                onRowsPerPageChange={handleChangeRowsPerPage}
            />
        </Paper>
    );
}