import { useEffect, useState } from "react";
import {
  Pagination,
  PaginationContent,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
  PaginationEllipsis,
} from "../components/ui/pagination";

interface PaginatedListProps<T> {
  fetchPage: (page: number, pageSize: number) => Promise<{
    items: T[];
    totalPages: number;
  }>;
renderItem: (item: T, index: number) => React.ReactNode;
  pageSize?: number;
  className?: string;
}

export default function PaginatedList<T>({
  fetchPage,
  renderItem,
  pageSize = 10,
  className,
}: PaginatedListProps<T>) {
  const [items, setItems] = useState<T[]>([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState<number | null>(null);

  useEffect(() => {
    const load = async () => {
      try {
        const data = await fetchPage(page, pageSize);
        setItems(data.items);
        setTotalPages(data.totalPages);
      } catch (err) {
        console.error("Pagination fetch error:", err);
      }
    };
    load();
  }, [page, pageSize, fetchPage]);

  const renderPageLinks = () => {
    const items = [];
    if (!totalPages) return null;

    for (let i = 1; i <= totalPages; i++) {
      if (i === page || i <= 3 || i === totalPages || Math.abs(i - page) <= 1) {
        items.push(
          <PaginationItem key={i}>
            <PaginationLink
              size="default"
              isActive={i === page}
              onClick={() => setPage(i)}
              href="#"
            >
              {i}
            </PaginationLink>
          </PaginationItem>
        );
      } else if (items[items.length - 1]?.type !== PaginationEllipsis) {
        items.push(
          <PaginationItem key={`ellipsis-${i}`}>
            <PaginationEllipsis />
          </PaginationItem>
        );
      }
    }
    return items;
  };

    return (
    <div className="flex flex-col gap-6">
      <div className={className}>
        {items.map((item, index) => (
      <div key={index}>{renderItem(item, index)}</div>
      ))}
      </div>

      {totalPages && totalPages > 1 && (
        <div className="flex justify-center">
          <Pagination>
            <PaginationContent>
              <PaginationItem>
                <PaginationPrevious
                  size="default"
                  href="#"
                  onClick={() => page > 1 && setPage(page - 1)}
                />
              </PaginationItem>
              {renderPageLinks()}
              <PaginationItem>
                <PaginationNext
                  size="default"
                  href="#"
                  onClick={() => page < totalPages && setPage(page + 1)}
                />
              </PaginationItem>
            </PaginationContent>
          </Pagination>
        </div>
      )}
    </div>
  );
}
