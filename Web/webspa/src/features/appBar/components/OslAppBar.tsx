import { AppBar, IconButton, ListItemIcon, ListItemText, Menu, MenuItem, Toolbar, Typography } from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import { useRef, useState } from "react";
import { FileOpen } from "@mui/icons-material";

export const OslAppBar = () => {

    const [anchorMenu, setAnchorMenu] = useState<null | HTMLElement>(null)

    const inputFile = useRef<HTMLInputElement | null>(null);

    const handleToggleMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorMenu(event.currentTarget);
    };
    const handleCloseMenu = () => {
        setAnchorMenu(null);
    };

    const onButtonClick = () => {
        inputFile.current?.click();
    };

    return (
        <>
            <input type='file' id='file' ref={inputFile} style={{ display: 'none' }} />
            <AppBar position="static">
                <Toolbar>
                    <IconButton
                        size="large"
                        edge="start"
                        color="inherit"
                        aria-label="menu"
                        sx={{ mr: 2 }}
                        onClick={handleToggleMenu}
                    >
                        <MenuIcon />
                    </IconButton>
                    <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                        OpenSportLog
                    </Typography>
                </Toolbar>
            </AppBar>
            <Menu
                id="basic-menu"
                anchorEl={anchorMenu}
                MenuListProps={{
                    'aria-labelledby': 'basic-button',
                }}
                open={Boolean(anchorMenu)}
                onClose={handleCloseMenu}
            >
                <MenuItem onClick={onButtonClick}>
                    <ListItemIcon>
                        <FileOpen fontSize="small" />
                    </ListItemIcon>
                    <ListItemText>Importer</ListItemText>
                </MenuItem>
            </Menu>
        </>);
}
