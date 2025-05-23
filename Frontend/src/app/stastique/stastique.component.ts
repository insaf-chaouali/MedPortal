import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { GoogleChartsModule, ChartType } from 'angular-google-charts';
import { StatistiqueService, Statistique } from '../services/statistique.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-stastique',
  standalone: true,
  imports: [RouterModule, CommonModule, GoogleChartsModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './stastique.component.html',
  styleUrls: ['./stastique.component.css']
})
export class StastiqueComponent {
  chart = {
    title: 'Évolution quotidienne des médecins',
    type: ChartType.ColumnChart,
    data: [] as any[],
    columnNames: ['Jours', 'Médecins créés'],
    options: {
      hAxis: { title: 'Jours' },
      vAxis: { title: 'Nombre de médecins' },
      colors: ['#007bff'],
      backgroundColor: '#f4f4f4'
    },
    width: 800,
    height: 400
  };
  pieChart = {
    title: 'Répartition des médecins par spécialité',
    type: ChartType.PieChart,
    data: [] as any[],
    columnNames: ['Spécialité', 'Nombre de médecins'],
    options: {
      pieHole: 0.4, // Donut style, tu peux enlever si tu veux un vrai cercle
      colors: ['#3366CC', '#DC3912', '#FF9900', '#109618', '#990099'],
      backgroundColor: '#f4f4f4',
    },
    width: 800,
    height: 400
  };
  constructor(private router: Router, private statService: StatistiqueService) {}

  ngOnInit() {
    this.statService.getStats().subscribe((res: Statistique[]) => {
      this.chart.data = res.map(stat => [stat.jours, stat.nombre]);
    });
    this.statService.getStatsParSpecialite().subscribe((res: { specialite: string, nombre: number }[]) => {
      this.pieChart.data = res.map(item => [item.specialite || 'Non définie', item.nombre]);
    });
  }

  logout(event: Event) {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
