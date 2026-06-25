package com.example.pharmadicmobile.adapters;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import com.example.pharmadicmobile.R;
import com.example.pharmadicmobile.dtos.MedicineDto;
import com.google.android.material.button.MaterialButton;
import com.google.android.material.chip.Chip;
import com.google.android.material.chip.ChipGroup;
import java.util.List;

public class MedicineAdapter extends RecyclerView.Adapter<MedicineAdapter.ViewHolder> {

    private List<MedicineDto> medicineList;
    private OnItemClickListener listener;

    public interface OnItemClickListener {
        void onItemClick(MedicineDto medicine);
    }

    public MedicineAdapter(List<MedicineDto> medicineList, OnItemClickListener listener) {
        this.medicineList = medicineList;
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_medicine, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        MedicineDto medicine = medicineList.get(position);
        holder.tvMedicineName.setText(medicine.getMedicineName());
        
        // Cài đặt Dosage và Contraindications
        String dosage = medicine.getDosage();
        holder.tvDosage.setText(holder.itemView.getContext().getString(R.string.dosage_label, 
                (dosage != null && !dosage.isEmpty()) ? dosage : "Chưa có thông tin"));
        
        String contra = medicine.getContraindications();
        holder.tvContraindications.setText(holder.itemView.getContext().getString(R.string.contraindication_label, 
                (contra != null && !contra.isEmpty()) ? contra : "Không có"));

        // Xử lý ảnh: Hiện tại dùng ảnh mặc định là logo app trong XML.
        // Nếu sau này dùng Glide/Picasso, bạn có thể load medicine.getImageUrl() vào holder.ivMedicine
        if (medicine.getImageUrl() == null || medicine.getImageUrl().isEmpty()) {
            holder.ivMedicine.setImageResource(R.mipmap.ic_launcher);
        }

        // Cài đặt Tags (sử dụng CategoryName làm chip mẫu)
        holder.chipGroupTags.removeAllViews();
        if (medicine.getCategoryName() != null && !medicine.getCategoryName().isEmpty()) {
            Chip chip = new Chip(holder.itemView.getContext());
            chip.setText(medicine.getCategoryName());
            chip.setChipBackgroundColorResource(R.color.surface_container_low);
            chip.setChipStrokeWidth(0);
            chip.setTextSize(11);
            holder.chipGroupTags.addView(chip);
        }

        holder.btnViewDetail.setOnClickListener(v -> {
            if (listener != null) listener.onItemClick(medicine);
        });
        
        holder.itemView.setOnClickListener(v -> {
            if (listener != null) listener.onItemClick(medicine);
        });
    }

    @Override
    public int getItemCount() {
        return medicineList != null ? medicineList.size() : 0;
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView tvMedicineName, tvDosage, tvContraindications;
        ImageView ivMedicine;
        ChipGroup chipGroupTags;
        MaterialButton btnViewDetail;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            tvMedicineName = itemView.findViewById(R.id.tvMedicineName);
            tvDosage = itemView.findViewById(R.id.tvDosage);
            tvContraindications = itemView.findViewById(R.id.tvContraindications);
            ivMedicine = itemView.findViewById(R.id.ivMedicine);
            chipGroupTags = itemView.findViewById(R.id.chipGroupTags);
            btnViewDetail = itemView.findViewById(R.id.btnViewDetail);
        }
    }
}
