import { Star } from "lucide-react";
import { useState } from "react";

type Props = {
  value: number;
  isUserRating?: boolean;
  canRate?: boolean;
  onRate?: (val: number) => void;
};

export default function RatingStars({ value, isUserRating, canRate, onRate }: Props) {
  const [selected, setSelected] = useState(value);
  const [hovered, setHovered] = useState<number | null>(null);
  const [localUserRated, setLocalUserRated] = useState(isUserRating ?? false);

  const activeColor = localUserRated ? "text-yellow-400" : "text-blue-400";

  const handleClick = (val: number) => {
    if (!canRate) return;
    setSelected(val);
    setLocalUserRated(true);
    onRate?.(val);
  };

  return (
    <div className="flex justify-center gap-1">
      {[1, 2, 3, 4, 5].map((i) => {
        const isActive = hovered !== null ? i <= hovered : i <= selected;

        return (
          <Star
            key={i}
            className={`w-5 h-5 transition-colors duration-150
              ${isActive ? `${activeColor} fill-current` : "stroke-gray-400"}
              ${canRate ? "cursor-pointer hover:scale-110" : "cursor-default opacity-60"}`}
            onMouseEnter={() => canRate && setHovered(i)}
            onMouseLeave={() => canRate && setHovered(null)}
            onClick={() => handleClick(i)}
          />
        );
      })}
    </div>
  );
}
