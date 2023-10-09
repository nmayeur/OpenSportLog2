import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow } from "@mui/material";
import React from "react";
import useFetchActivities from "../hooks/FetchActivities";
import { ActivitiesListRow } from "./ActivitiesListRow";
import { ITableColumns } from "../../../types/ITableColumns";

interface ActivitiesListProps {
    athleteId: number;
    onActivityIdChange: (activityId: number) => void
}

export const ActivitiesList = (props: ActivitiesListProps) => {

    const columns: readonly ITableColumns[] = [
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
    const [url] = React.useState(`https://osl-webapiapi-dev.azure-api.net/osl-dev/api/Activity/activitiesByAthlete?athleteId=${props.athleteId}&pageSize=10&pageIndex=0`);
    const [api_key] = React.useState("2d5915334aa74fb19fefe972c952c5d6");

    const handleChangePage = (_event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(+event.target.value);
        setPage(0);
    };

    const { rows } = useFetchActivities(url, api_key);

    const rowOnClick = (row: MouseEvent) => {
        const test = row.target
        console.log(test?`Row clicked ${1}`:"");

    }

    return (
        <Paper sx={{ width: '100%', overflow: 'hidden' }} elevation={3}>
            <TableContainer sx={{ maxHeight: "max-content" }}>
                <Table stickyHeader aria-label="header" size="small">
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
                                    <TableRow hover role="checkbox" tabIndex={-1} key={row.id} onClick={rowOnClick}>
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