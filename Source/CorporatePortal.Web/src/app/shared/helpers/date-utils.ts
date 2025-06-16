export const monthMap: { [key: string]: number } = {
    'січня': 0, 'лютого': 1, 'березня': 2, 'квітня': 3, 'травня': 4, 'червня': 5,
    'липня': 6, 'серпня': 7, 'вересня': 8, 'жовтня': 9, 'листопада': 10, 'грудня': 11
  };

  export function parseUkrDateLabel(label: string): Date {
    const [dayStr, monthStr] = label.split(' ');
    const day = parseInt(dayStr, 10);
    const month = monthMap[monthStr];
    return new Date(new Date().getFullYear(), month, day);
  }

  export function normalizeDate(d: Date): Date {
    const date = new Date(d);
    date.setHours(0, 0, 0, 0);
    return date;
  }

  export function formatBirthdayLabel(date: Date): string {
    return date.toLocaleDateString('uk-UA', {
      day: 'numeric',
      month: 'long'
    });
  }