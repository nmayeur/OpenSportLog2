export interface TableColumns {
    id: 'id' | 'name' | 'athlete' | 'location';
    label: string;
    minWidth?: number;
    align?: 'right';
    format?: (value: number) => string;
}
