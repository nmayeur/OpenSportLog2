export interface IPaginatedItemsViewModel<E> {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: E[]
}
